using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkPerformedAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkPerformedEndPoints;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetWorkPerformedEndPoint : EndpointBase
{
  private readonly IWorkPerformedService _workPerformedService;

  public GetWorkPerformedEndPoint(IWorkPerformedService workPerformedService)
  {
    _workPerformedService = workPerformedService;
  }

  [HttpGet("calendar/workperformed/{dispatchNo}")]
  public async Task<ActionResult<WorkPerformedResponse>> HandleAsync(long dispatchNo, [FromQuery] GetWorkPerformedAddressRequest getWorkPerformedAddress, CancellationToken cancellationToken = default)
  {
    if (dispatchNo == null || dispatchNo == 0)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchNo), ErrorMessage = $"Dispatch number is required" }
      };

      return BadRequest(Result<WorkPerformedResponse>.Invalid(errors));
    }

    if (getWorkPerformedAddress == null || string.IsNullOrEmpty(getWorkPerformedAddress.GeoLat) || string.IsNullOrEmpty(getWorkPerformedAddress.GeoLon))
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = "geoLon, geoLat" , ErrorMessage = $"Geo coordinates are required" }
      };

      return BadRequest(Result<WorkPerformedResponse>.Invalid(errors));
    }

    var result = await _workPerformedService.GetWorkPerformed(dispatchNo, getWorkPerformedAddress);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
