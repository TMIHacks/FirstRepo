using System.Text.Json;
using System.Threading;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JSAAggregate;
using ConeConnect.Calendar.Core.WorkSitePhotosAggregate;
using ConeConnect.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using RestSharp;

namespace ConeConnect.Calendar.API.Controllers.WorkSitePhotosEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AddWorkSitePhotosEndPoint : EndpointBaseAsync.WithRequest<WorkSitePhotosRequest>.WithActionResult<WorkSitePhotosResponse>
{
  private readonly IWorkSitePhotosService _workSitePhotosService;
  private IConfiguration _config;

  public AddWorkSitePhotosEndPoint(IConfiguration config, IWorkSitePhotosService workSitePhotosService)
  {
    _config = config;
    _workSitePhotosService = workSitePhotosService;
  }

  [HttpPost("calendar/addworkphotos")]
  public override async Task<ActionResult<WorkSitePhotosResponse>> HandleAsync(WorkSitePhotosRequest photosRequest, CancellationToken cancellationToken = default)
  {
    if (photosRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(photosRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<WorkSitePhotosResponse>.Invalid(errors));
    }

    var result = await _workSitePhotosService.AddWorkSitePhotos(photosRequest);
    await _workSitePhotosService.FinalizeWorkSitePhotosUpload(result,photosRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
