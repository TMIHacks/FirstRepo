using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DeviceTrackingAggrigate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.Services;
using ConeConnect.Calendar.Core.WorkSitePhotosAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.TrackingEndPoints;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AddDeviceTrackingController : EndpointBaseAsync.WithRequest<DeviceTrackingDTO>.WithActionResult<DeviceTrackingDTO>
{
  private readonly IDeviceTrackingService _deviceTrackingService;
  public AddDeviceTrackingController(IDeviceTrackingService deviceTrackingService)
  { 
   _deviceTrackingService= deviceTrackingService;
  }

  [HttpPost("calendar/AddDeviceTracking")]
  public override async Task<ActionResult<DeviceTrackingDTO>> HandleAsync(DeviceTrackingDTO deviceTrackingDTO, CancellationToken cancellationToken = default)
  {
    if (deviceTrackingDTO == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(deviceTrackingDTO), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<WorkSitePhotosResponse>.Invalid(errors));
    }

    var result = await _deviceTrackingService.AddDeviceTracking(deviceTrackingDTO); 

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
