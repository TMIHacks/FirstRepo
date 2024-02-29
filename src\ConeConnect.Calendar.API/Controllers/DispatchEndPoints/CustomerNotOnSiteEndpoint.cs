using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.User.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;

public class CustomerNotOnSiteEndpoint : EndpointBase
{

  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public CustomerNotOnSiteEndpoint(IDispatchService dispatchService
    , IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  }

  [HttpPost("Calendar/CutomerNotOnSite/{dispatchId}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<DispatchScheduleByIdDTO>> HandleAsync(int dispatchId)
  {     
    var result = await _dispatchService.CustomerNotOnSite(dispatchId); 
    return new OkObjectResult(result.GetValue());
  }

  [HttpPost("Calendar/ExecuteCustomerNotOnSitePushNotificaion")]
  [HeaderFilterAttribute()]
  public async Task<ActionResult<bool>> HandleAsync()
  {
    Task.Run(() =>
    {
      _dispatchService.ExecuteCustomerNotOnSitePushNotificaion();
    });
    return new OkObjectResult(true);
  }
}
