using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JobSchedulesEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TravelStartEndPoint : EndpointBaseAsync.WithRequest<TravelStartRequest>.WithActionResult<JobSchedulesResponse>
{
  private readonly IJobSchedulesService _jobSchedulesService;

  public TravelStartEndPoint(IJobSchedulesService jobSchedulesService)
  {
    _jobSchedulesService = jobSchedulesService;
  }

  [HttpPost("calendar/travelstart")]
  public override async Task<ActionResult<JobSchedulesResponse>> HandleAsync(TravelStartRequest travelStartRequest, CancellationToken cancellationToken = default)
  {
    if (travelStartRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(travelStartRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    if (travelStartRequest.TravelStart == null || travelStartRequest.TravelStart == DateTimeOffset.MinValue)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(travelStartRequest.TravelStart), ErrorMessage = $"Travel start date is required" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    var result = await _jobSchedulesService.TravelStart(travelStartRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
