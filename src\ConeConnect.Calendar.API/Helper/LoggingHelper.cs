using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Serilog;
using Serilog.Core;
using Microsoft.Azure.Cosmos;
using SumoLogic.Logging.Serilog.Extensions;
using Newtonsoft.Json;


namespace ConeConnect.Calendar.API.Helpers
{
  public class LoggingHelper
  {
    private const string _isImageString64Pattern = @"((IVBOR)|(/9J/4)|(AAAAF)|(JVBER))(?:[a-zA-Z0-9+\/]{4})*(?:|(?:[a-zA-Z0-9+\/]{3}=)|(?:[a-zA-Z0-9+\/]{2}==)|(?:[a-zA-Z0-9+\/]{1}===))";
    private readonly Logger sumoLogicLog;
    //private readonly string sourceName = "Orange-Calendar-Dev";
    //private readonly string sourceCategory = "Orange-Calendar-Dev";
    //public const string SumoLogicUri = "https://collectors.us2.sumologic.com/receiver/v1/http/ZaVnC4dhaV2Iw6RgoEp3oeCqtEr6GZ61we5N1aXolfPcxoD20tdYvMRWQdMErwXRhBTzbJOYdNEQzuJ4TztV9OlmTLZAElGaNiobtz4gr9i1T6LMsPOlxA==";
    private IConfiguration _config;
    private int nLogLevel = 0;

    public LoggingHelper(IConfiguration config)
    {
      _config = config;

      var SumoLogicUri = _config.GetSection("SumoLogicUri").Value;
      nLogLevel = _config.GetSection("SumoLogicLogLevel").Value != null ? Convert.ToInt32(_config.GetSection("SumoLogicLogLevel").Value) : 2;
      var sourceName = _config.GetSection("SumoLogicSourceName").Value;
      var sourceCategory = _config.GetSection("SumoLogicSourceCategory").Value;

      sumoLogicLog = new LoggerConfiguration()
                 .WriteTo
                 .BufferedSumoLogic(
                          new Uri(SumoLogicUri),
                          sourceName: sourceName,
                          sourceCategory: sourceCategory,
                          sourceHost: $"{Dns.GetHostName()}",
                          connectionTimeout: 50000,
                          retryInterval: 5000,
                          messagesPerRequest: 10,
                          maxFlushInterval: 10000,
                          flushingAccuracy: 250,
                          maxQueueSizeBytes: 500000)
                 .CreateLogger();
    }

    #region Singlton Accessor
    //private static LoggingHelper _instance = null;

    //public static LoggingHelper instance
    //{
    //  get { return _instance ?? (_instance = new LoggingHelper()); }
    //}

    #endregion

    /// <summary>
    /// sends an error message to sumo logic
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <param name="options">Options for the request</param>
    public void error(string msg, LoggingRequestOptions options)
    {
      send(msg, "Error", options);
    }

    /// <summary>
    /// sends an exception message to sumo logic
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <param name="options">Options for the request</param>
    public void exception(string msg, LoggingRequestOptions options)
    {
      send(msg, "Exception", options);
    }

    /// <summary>
    /// sends an warning message to sumo logic
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <param name="options">Options for the request</param>
    public void warning(string msg, LoggingRequestOptions options)
    {
      send(msg, "Warning", options);
    }

    /// <summary>
    /// sends an informational message to sumo logic
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <param name="options">Options for the request</param>
    public void info(string msg, LoggingRequestOptions options)
    {
      send(msg, "Info", options);
    }

    private void send(string msg, string type, LoggingRequestOptions options)
    {
      if (nLogLevel == 3 || (nLogLevel == 2 && (type == "Exception" || type == "Error")) || (nLogLevel == 1 && (type == "Info" || type == "Warning")))
      {
        msg = type + Environment.NewLine + Environment.NewLine + msg;

        if (options.addTimeStampToMsg)
        {
          msg = "[" + DateTime.Now + "]" + Environment.NewLine + Environment.NewLine + msg;
        }
        SendToSumoLogic(msg);
      }
    }

    public void SendToSumoLogic(string message)
    {
      if (!string.IsNullOrWhiteSpace(message))
      {
        LogSumoLogic(new string[] { message });
      }
    }

    public void LogSumoLogic(string[] messages)
    {
      foreach (var message in messages)
      {
        string msg = string.Empty;

        try
        {
          // Remove stringBase64.
          msg = Regex.Replace(message, _isImageString64Pattern, "", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(.5));
        }
        catch { }

        sumoLogicLog.Information(msg);
      }
    }

    public static string BuildMethodInfoString(object methodData, string info = "")
    {
      string msg = string.Empty;

      msg += Environment.NewLine;
      msg += Environment.NewLine;
      msg += "--------------------------  Method / Model Information  --------------------------";
      msg += Environment.NewLine;

      if (!string.IsNullOrEmpty(info))
      {
        msg += Environment.NewLine;
        msg += info;
        msg += Environment.NewLine;
      }

      msg += Environment.NewLine;
      msg += JsonConvert.SerializeObject(methodData, Formatting.Indented) + Environment.NewLine;
      msg += Environment.NewLine;
      msg += "----------------------------------------------------------------------------------";
      msg += Environment.NewLine;
      msg += Environment.NewLine;
      return msg;
    }
    public static string GetExceptionMessage(Exception ex, string message)
    {
      var errorMessage = message + Environment.NewLine;
      errorMessage += ex.Message + Environment.NewLine;
      errorMessage += ex.StackTrace + Environment.NewLine;
      if (ex.InnerException != null)
      {
        errorMessage += "Inner exception:" + Environment.NewLine;
        errorMessage += ex.InnerException.Message + Environment.NewLine;
        errorMessage += ex.InnerException.StackTrace;
      }
      return errorMessage;
    }

  }

  public abstract class LoggingRequestOptionsModel
  {
    public bool addTimeStampToMsg { get; set; }
    public string applicationName { get; set; }
    public string sumoLogicUrl { get; set; }
    public void validate()
    {
      if (string.IsNullOrEmpty(applicationName)) throw new ArgumentException("Application Name is required");
    }
  }

  public class LoggingRequestOptions : LoggingRequestOptionsModel
  {
    public static LoggingRequestOptions Defaults
    {
      get
      {
        return new LoggingRequestOptions()
        {
          addTimeStampToMsg = true,
          applicationName = ""
        };
      }
    }
  }
}
