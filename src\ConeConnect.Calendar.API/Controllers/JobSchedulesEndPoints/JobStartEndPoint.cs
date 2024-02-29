using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JobSchedulesEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class JobStartEndPoint : EndpointBaseAsync.WithRequest<JobStartRequest>.WithActionResult<JobSchedulesResponse>
{
  private readonly IJobSchedulesService _jobSchedulesService;

  public JobStartEndPoint(IJobSchedulesService jobSchedulesService)
  {
    _jobSchedulesService = jobSchedulesService;
  }

  [HttpPost("calendar/jobstart")]
  public override async Task<ActionResult<JobSchedulesResponse>> HandleAsync(JobStartRequest jobStartRequest, CancellationToken cancellationToken = default)
  {
    if (jobStartRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(jobStartRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    if (jobStartRequest.OnJob == null || jobStartRequest.OnJob == DateTimeOffset.MinValue)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(jobStartRequest.OnJob), ErrorMessage = $"On job date is required" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    var result = await _jobSchedulesService.JobStart(jobStartRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
