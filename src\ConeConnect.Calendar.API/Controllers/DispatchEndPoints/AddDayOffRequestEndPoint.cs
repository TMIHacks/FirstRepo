using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class AddDayOffRequestEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public AddDayOffRequestEndPoint(IDispatchService dispatchService, IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  }

  [HttpPost("Calendar/DayOffRequests")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<CreateDayOffItem>> HandleAsync(CreateDayOffRequest createDayOffItem)
  {
    CreateDayOffItem createDispatchItem = await _dispatchService.AddDayOffRequest(createDayOffItem);
    return Ok(createDispatchItem);
  }

  [HttpGet("Calendar/GetAllDayOffRequests")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<List<CreateDayOffItem>>> HandleAsync()
  {
    var data = await _dispatchService.GetAllDayOffRequests();
    return Ok(data.Value);
  }

  [HttpPut("Calendar/DeleteDayOffRequest/{requestDayOffId}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> DeleteDayOffRequests([FromRoute] string requestDayOffId)
  {
    var data = await _dispatchService.DeleteDayOffRequests(requestDayOffId);
    return Ok(data.Value);
  }


  [HttpPut("Calendar/UpdateAllRejectedRecord")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> GetAllRejectedCounts()
  {
    var data = await _dispatchService.GetAllRejectedCount();
    return Ok(data.Value);
  }
}
