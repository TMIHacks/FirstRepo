using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JobSchedulesEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ShiftStartEndPoint : EndpointBaseAsync.WithRequest<ShiftStartRequest>.WithActionResult<JobSchedulesResponse>
{
  private readonly IJobSchedulesService _jobSchedulesService;

  public ShiftStartEndPoint(IJobSchedulesService jobSchedulesService)
  {
    _jobSchedulesService = jobSchedulesService;
  }

  [HttpPost("calendar/shiftstart")]
  public override async Task<ActionResult<JobSchedulesResponse>> HandleAsync(ShiftStartRequest shiftStartRequest, CancellationToken cancellationToken = default)
  {
    if (shiftStartRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(shiftStartRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    if (shiftStartRequest.ShiftStartedOn == null || shiftStartRequest.ShiftStartedOn == DateTimeOffset.MinValue)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(shiftStartRequest.ShiftStartedOn), ErrorMessage = $"Shift started on date is required" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    var result = await _jobSchedulesService.ShiftStart(shiftStartRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
