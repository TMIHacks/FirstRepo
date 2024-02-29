using System.Net;
using System.Text.Json;
using ConeConnect.Calendar.API.Helpers;

namespace ConeConnect.Calendar.API.Middleware;
public class ErrorHandlerMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ErrorHandlerMiddleware> _ilogger;
  private IConfiguration _config;
  public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> ilogger, IConfiguration config)
  {
    _next = next;
    _ilogger = ilogger;
    _config = config;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception error)
    {
      var response = context.Response;
      response.ContentType = "application/json";
      int errorCode = 0;
      switch (error)
      {
        case KeyNotFoundException e:
          response.StatusCode = (int)HttpStatusCode.NotFound;
          errorCode = (int)HttpStatusCode.NotFound;
          break;
        default:
          response.StatusCode = (int)HttpStatusCode.InternalServerError;
          errorCode = (int)HttpStatusCode.InternalServerError;
          break;
      }

      _ilogger.LogError(error?.Message);
      _ilogger.LogError(error?.StackTrace);
      var result = JsonSerializer.Serialize(new { message = error?.Message, errorCode = errorCode, StackTrace = error?.StackTrace, isSuccess = false });

      var options = LoggingRequestOptions.Defaults;
      //var sumoLogicLog = new LoggingHelper(_config);
      //sumoLogicLog.error(result.ToString(), options);


      await response.WriteAsync(result);
    }
  }
}
