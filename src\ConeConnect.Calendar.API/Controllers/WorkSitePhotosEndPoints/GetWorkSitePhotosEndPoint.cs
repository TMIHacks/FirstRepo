using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkReceiptAggregate;
using ConeConnect.Calendar.Core.WorkSitePhotosAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkSitePhotosEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetWorkSitePhotosEndPoint : EndpointBaseAsync.WithRequest<long>.WithActionResult<WorkSitePhotosResponse>
{
  private readonly IWorkSitePhotosService _workSitePhotosService;

  public GetWorkSitePhotosEndPoint(IWorkSitePhotosService workSitePhotosService)
  {
    _workSitePhotosService = workSitePhotosService;
  }

  [HttpGet("calendar/workphotos/{dispatchNo}")]
  public override async Task<ActionResult<WorkSitePhotosResponse>> HandleAsync(long dispatchNo, CancellationToken cancellationToken = default)
  {
    if (dispatchNo == null || dispatchNo == 0)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchNo), ErrorMessage = $"Dispatch number is required" }
      };

      return BadRequest(Result<WorkSitePhotosResponse>.Invalid(errors));
    }

    var result = await _workSitePhotosService.GetWorkSitePhotos(dispatchNo);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new List<WorkSitePhotosResponse>());

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
