using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JobSchedulesEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class JobEndEndPoint : EndpointBaseAsync.WithRequest<JobEndRequest>.WithActionResult<JobSchedulesResponse>
{
  private readonly IJobSchedulesService _jobSchedulesService;

  public JobEndEndPoint(IJobSchedulesService jobSchedulesService)
  {
    _jobSchedulesService = jobSchedulesService;
  }

  [HttpPost("calendar/jobend")]
  public override async Task<ActionResult<JobSchedulesResponse>> HandleAsync(JobEndRequest jobEndRequest, CancellationToken cancellationToken = default)
  {
    if (jobEndRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(jobEndRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    if (jobEndRequest.LeaveJob == null || jobEndRequest.LeaveJob == DateTimeOffset.MinValue)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(jobEndRequest.LeaveJob), ErrorMessage = $"Leave job date is required" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    var result = await _jobSchedulesService.JobEnd(jobEndRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
