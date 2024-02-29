using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JobSchedulesEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetEndPoint : EndpointBaseAsync.WithRequest<long>.WithActionResult<JobSchedulesResponse>
{
  private readonly IJobSchedulesService _jobSchedulesService;

  public GetEndPoint(IJobSchedulesService jobSchedulesService)
  {
    _jobSchedulesService = jobSchedulesService;
  }


  [HttpGet("calendar/jobwork/{dispatchNo}")]
  public override async Task<ActionResult<JobSchedulesResponse>> HandleAsync(long dispatchNo, CancellationToken cancellationToken = default)
  {
    if (dispatchNo == null || dispatchNo == 0)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchNo), ErrorMessage = $"Dispatch number is required" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    var result = await _jobSchedulesService.GetJobWork(dispatchNo);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
