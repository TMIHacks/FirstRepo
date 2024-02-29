using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class AddWeekendShiftRequestEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public AddWeekendShiftRequestEndPoint(IDispatchService dispatchService, IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  }

  [HttpPost("Calendar/WeekendShiftRequest")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<CreateWeekendShiftItem>> HandleAsync(CreateWeekendShiftRequest createWeekendShift)
  {
    CreateWeekendShiftItem createWeekendShiftItem = await _dispatchService.AddWeekendShiftRequest(createWeekendShift);
    return Ok(createWeekendShiftItem);
  }
 

}

