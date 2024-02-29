using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;

public class Z_Helpers : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;
  private readonly INotificationService _notificationService;

  public Z_Helpers(IDispatchService dispatchService
    , IUserDetailService userDetailService,
INotificationService notificationService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
    _notificationService = notificationService;
  }

  [HttpPost("Calendar/SendPushNotification")]
  public async Task<ActionResult<string>> HandleAsync(string token, string title, string body, string dispachNo)
  {
    var result = await _notificationService.SendPushNotification(new List<string> { token }, title, body, dispachNo);

    return new OkObjectResult(result);
  }
  [HttpGet("Calendar/ConvertDateToTimestamp")]
  public ActionResult<long> HandleAsync(DateTime date)
  {
    var result = _dispatchService.ConvertDateToTimestamp(date);

    return new OkObjectResult(result);
  }
  [HttpGet("Calendar/FormatDispatchDate")]
  public ActionResult<DateTime> HandleAsync(long date)
  {
    var result = _dispatchService.FormatDispatchDate(date);

    return new OkObjectResult(result);
  }
}



