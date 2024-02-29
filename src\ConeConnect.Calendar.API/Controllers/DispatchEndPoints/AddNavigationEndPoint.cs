using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class AddNavigationEndPoint : EndpointBase
{
  private readonly INavigationService _navigationService;
  private readonly IUserDetailService _userDetailService;

  public AddNavigationEndPoint(INavigationService navigationService,IUserDetailService userDetailService)
  {
    _navigationService = navigationService;
    _userDetailService = userDetailService;
  }

  [HttpPost("Calendar/AddNavigation")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<NavigationIteam>> HandleAsync(NavigationRequest navigationItem)
  {
    NavigationIteam navigation = await _navigationService.AddNavigation(navigationItem);
    return Ok(navigation);
  }

  [HttpGet("Calendar/GetNavigationByDispatch/{dispatchNo}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<List<NavigationIteam>>> HandleAsync([FromRoute] int dispatchNo)
  {
    var data = await _navigationService.GetNavigationByDispatch(dispatchNo);
    return Ok(data.Value);
  }
}
