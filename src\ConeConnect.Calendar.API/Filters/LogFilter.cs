using ConeConnect.Calendar.API.Models;
using ConeConnect.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConeConnect.Calendar.API.Filters;

public class LogFilter : IActionFilter
{
  private readonly ILogger<LogFilter> _ilogger;
  private readonly string strGUID;

  public LogFilter(ILogger<LogFilter> ilogger)
  {
    _ilogger = ilogger;
    strGUID = Guid.NewGuid().ToString();
  }

  public async void OnActionExecuting(ActionExecutingContext context)
  {
    var objLog = new
    {
      AactionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName,
      controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName,
      Model = context.ActionArguments
    };

    LogRequest obj = new LogRequest();
    obj.Id = strGUID;
    obj.ControllerName = objLog.controllerName.ToString();
    obj.Model = Newtonsoft.Json.JsonConvert.SerializeObject(objLog);
    obj.Type = "Request";
    DBHelper<LogRequest>.DatabaseId = "ConeConnectLoggerDB";
    await DBHelper<LogRequest>.CreateContainerAsync("APIRequestLog", "/Type");
    await DBHelper<LogRequest>.AddItemsToContainerAsync(obj, obj.Id, obj.Type);

    //_ilogger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(objLog));
  }

  public async void OnActionExecuted(ActionExecutedContext context)
  {
    if (context.Result != null)
    {
      _ilogger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(context.Result));

      LogRequest obj = new LogRequest();
      obj.Id = strGUID;
      obj.Model = Newtonsoft.Json.JsonConvert.SerializeObject(context.Result);
      obj.Type = "Response";
      DBHelper<LogRequest>.DatabaseId = "ConeConnectLoggerDB";
      await DBHelper<LogRequest>.CreateContainerAsync("APIResponseLog", "/Type");
      await DBHelper<LogRequest>.AddItemsToContainerAsync(obj, obj.Id, obj.Type);
    }
  }
}
