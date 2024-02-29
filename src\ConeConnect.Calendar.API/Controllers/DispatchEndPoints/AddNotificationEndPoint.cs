using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class AddNotificationEndPoint : EndpointBase
{
  private readonly INotificationService _notificationService;

  public AddNotificationEndPoint(INotificationService notificationService,
    IUserDetailService userDetailService)
  {
    _notificationService = notificationService;
  }

  [HttpPost("Calendar/AddNotification")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<NotificationIteam>> HandleAsync(NotificationRequest notificationRequest)
  {
    NotificationIteam notification = await _notificationService.AddNotificationRequest(notificationRequest);
    return Ok(notification);
  }

  [HttpGet("Calendar/GetNotification")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<NotificationIteamResponce>> HandleAsync()
  {
    var data = await _notificationService.GetNotifications();
    return Ok(data.Value);
  }


  [HttpPost("Calendar/UpdateNotification")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> HandleAsync(NotificationUpdate notificationUpdate)
  {
    var data = await _notificationService.UpdateNotification(notificationUpdate);
    return Ok(data.Value);
  }
}
