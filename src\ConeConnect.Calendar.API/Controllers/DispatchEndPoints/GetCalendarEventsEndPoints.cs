using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class GetCalendarEventsEndPoints : EndpointBase
{
  private readonly IDispatchService _dispatchService;

  public GetCalendarEventsEndPoints(IDispatchService dispatchService)
  {
    _dispatchService = dispatchService;
  }

  [HttpGet("Calendar/GetCalendarEvents")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<CalendarEvents>> HandleAsync()
  {
    var result = await _dispatchService.GetCalendarEvents();
    return new OkObjectResult(result.GetValue());
  }

}
