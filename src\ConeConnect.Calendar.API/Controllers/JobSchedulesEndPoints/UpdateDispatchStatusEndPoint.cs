using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JobSchedulesAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JobSchedulesEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UpdateDispatchStatusEndPoint : EndpointBaseAsync.WithRequest<DispatchStatusRequest>.WithActionResult<JobSchedulesResponse>
{
  private readonly IJobSchedulesService _jobSchedulesService;

  public UpdateDispatchStatusEndPoint(IJobSchedulesService jobSchedulesService)
  {
    _jobSchedulesService = jobSchedulesService;
  }

  [HttpPost("calendar/tcdispatchstatus")]
  public override async Task<ActionResult<JobSchedulesResponse>> HandleAsync(DispatchStatusRequest dispatchStatusRequest, CancellationToken cancellationToken = default)
  {
    if (dispatchStatusRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchStatusRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    if (dispatchStatusRequest.DatetimeOn == null || dispatchStatusRequest.DatetimeOn == DateTimeOffset.MinValue)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchStatusRequest.DatetimeOn), ErrorMessage = $"Date is required" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    if (dispatchStatusRequest.Status == 0)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchStatusRequest.DatetimeOn), ErrorMessage = $"Status is required" }
      };

      return BadRequest(Result<JobSchedulesResponse>.Invalid(errors));
    }

    var result = await _jobSchedulesService.UpdateDispatchStatus(dispatchStatusRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
  [HttpPost("calendar/UpdateDispatchOffertcChoice")]
  public async Task<ActionResult<Result<bool>>> HandleAsync(DispatchOfferRequest dispatchOfferRequest, CancellationToken cancellationToken = default)
  {
    if (dispatchOfferRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchOfferRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<Boolean>.Invalid(errors));
    }
 
    if (dispatchOfferRequest.dispatchNo == 0)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchOfferRequest.dispatchNo), ErrorMessage = $"DispatchNo is required" }
      };

      return BadRequest(Result<Boolean>.Invalid(errors));
    }

    var result = await _jobSchedulesService.UpdateDispatchOffertcChoice(dispatchOfferRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}

