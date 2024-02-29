using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class GetDispatchNotificationEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public GetDispatchNotificationEndPoint(IDispatchService dispatchService, IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  } 

  [HttpGet("Calendar/GetNewDispatchDetails/{DispatchNo}")]
  public async Task<ActionResult<GetNewDispatchIteam>> HandleAsync(int DispatchNo)
  {
    var result = await _dispatchService.GetNewDispatchDetails(DispatchNo);
    return new OkObjectResult(result.GetValue());
  }
}
