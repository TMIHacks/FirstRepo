using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetDispatchEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public GetDispatchEndPoint(IDispatchService dispatchService
    , IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  }

  [HttpGet("Calendar/GetPendingCount")]
  public async Task<ActionResult<DispatchListCount>> HandleAsync(CancellationToken cancellationToken = default)
  {
    var result = await _dispatchService.GetPendingCount();
    return new OkObjectResult(result.GetValue());
  }


  [HttpGet("Calendar/dispatchCrew/{dispatchNo}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<DispatchCrewResponse>> HandleAsync(int dispatchNo)
  {
    var data = await _dispatchService.GetDispatchCrew(dispatchNo);
    return new OkObjectResult(data.GetValue());
  }
}
