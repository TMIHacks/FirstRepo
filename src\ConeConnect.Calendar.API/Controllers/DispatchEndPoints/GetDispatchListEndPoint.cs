using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.User.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;

public class GetDispatchListEndPoint : EndpointBaseAsync.WithRequest<DateTime?>.WithActionResult<List<DispatchPercantageIteamResponse>>
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;
  private readonly INotificationService _notificationService;

  public GetDispatchListEndPoint(IDispatchService dispatchService
    , IUserDetailService userDetailService,
    INotificationService notificationService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
    _notificationService = notificationService;
  }
   

  [HttpGet("Calendar/GetDispatchList")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public override async Task<ActionResult<List<DispatchPercantageIteamResponse>>> HandleAsync([FromQuery] DateTime? selectedDate, CancellationToken cancellationToken = default) 
  {
    var result = await _dispatchService.GetDispatchForTheDay(_userDetailService.userID, selectedDate);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new List<DispatchPercantageIteamResponse>());

    return new OkObjectResult(result.GetValue());
  }

  [HttpPost("Calendar/ExecuteNewDispatchPushNotifications")]
  [HeaderFilterAttribute()]
  public async Task<ActionResult<List<GetDispatchNotification>>> HandleAsync()
  {
    _ = Task.Run(() => _notificationService.ExecutePushNotification());
    return new OkObjectResult(true);
  }

   
}
