using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JobSchedulesEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ConfirmShiftEndEndPoint : EndpointBaseAsync.WithRequest<ConfirmShiftEndRequest>.WithActionResult<JobSchedulesResponse>
{
  private readonly IJobSchedulesService _jobSchedulesService;

  public ConfirmShiftEndEndPoint(IJobSchedulesService jobSchedulesService)
  {
    _jobSchedulesService = jobSchedulesService;
  }

  [HttpPost("calendar/confirmshiftend")]
  public override async Task<ActionResult<JobSchedulesResponse>> HandleAsync(ConfirmShiftEndRequest confirmShiftEnd, CancellationToken cancellationToken = default)
  {
    if (confirmShiftEnd == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(confirmShiftEnd), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    var result = await _jobSchedulesService.ConfirmShiftEnd(confirmShiftEnd);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
