using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetDispatchItemEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public GetDispatchItemEndPoint(IDispatchService dispatchService
    , IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  }

  [HttpGet("Calendar/Dispatch/{dispatchId}")]
  public  async Task<ActionResult<DispatchScheduleByIdDTO>> HandleAsync([FromRoute] string dispatchId)
  {

    if (string.IsNullOrEmpty(dispatchId))
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchId), ErrorMessage = $"Invalid dispatchId" }
      };

      return BadRequest(Result<DispatchScheduleByIdDTO>.Invalid(errors));
    }

    var result = await _dispatchService.GetDispatchDetails(_userDetailService.userID, dispatchId);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
