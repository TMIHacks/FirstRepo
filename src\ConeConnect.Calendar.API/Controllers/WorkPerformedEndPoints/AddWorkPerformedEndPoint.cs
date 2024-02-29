using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkPerformedAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkPerformedEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AddWorkPerformedEndPoint : EndpointBaseAsync.WithRequest<WorkPerformedRequest>.WithActionResult<WorkPerformedResponse>
{
  private readonly IWorkPerformedService _workPerformedService;

  public AddWorkPerformedEndPoint(IWorkPerformedService workPerformedService)
  {
    _workPerformedService = workPerformedService;
  }

  [HttpPost("calendar/addworkperformed")]
  public override async Task<ActionResult<WorkPerformedResponse>> HandleAsync(WorkPerformedRequest performedRequest, CancellationToken cancellationToken = default)
  {
    if (performedRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(performedRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<WorkPerformedResponse>.Invalid(errors));
    }

    var result = await _workPerformedService.AddWorkPerformed(performedRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
