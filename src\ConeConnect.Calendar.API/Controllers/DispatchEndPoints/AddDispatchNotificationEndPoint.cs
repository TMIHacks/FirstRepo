using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.User.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class AddDispatchNotificationEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;
  private readonly INotificationService _notificationService;

  public AddDispatchNotificationEndPoint(IDispatchService dispatchService,
    IUserDetailService userDetailService,
    INotificationService notificationService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
    _notificationService = notificationService;
  }

  [HttpPost("Calendar/Dispatch/AddDeviceToken")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<DiviceToken>> HandleAsync(DiviceTokenRequest createDayOffItem)
  {
    DiviceToken createDispatchItem = await _dispatchService.AddDiviceTokenRequest(createDayOffItem);
    return Ok(createDispatchItem);
  }
  [HttpPost("Calendar/GetDeviceTokenByEmpId")] 
  [HeaderFilterAttribute()]
  public async Task<ActionResult<DiviceToken>> HandleAsync(int empId)
  {
    DiviceToken createDispatchItem = await _dispatchService.GetNotificationByUserId(empId);
    return Ok(createDispatchItem);
  }

}
