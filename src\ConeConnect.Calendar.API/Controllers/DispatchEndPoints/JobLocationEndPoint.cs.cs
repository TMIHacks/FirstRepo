using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints; 
public class JobLocationEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService; 

  public JobLocationEndPoint(IDispatchService dispatchService)
  {
    _dispatchService = dispatchService; 
  }
  [HttpPost("Calendar/AddJobLocation")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<JobLocationRQ>> HandleAsync(JobLocationRQ jobLocation)
  {
    var  response = await _dispatchService.AddJobLocatonContainer(jobLocation);
    return Ok(response.Value);
  }

  [HttpGet("Calendar/GetJobLocation")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<List<JobLocationRQ>>> HandleAsync(int dispatchNo)
  {
    var response = await _dispatchService.GetJobLocationsByDispatchNo(dispatchNo);
    return Ok(response.Value);
  }
}
