using Ardalis.ApiEndpoints;
using Azure.Core;
using ConeConnect.Calendar.API.Helper;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Social.API.Controllers.SocialWallEndPoints;

public class GetRequestOffsEndPoints : EndpointBaseAsync.WithRequest<RequestOffRequestDTO>.WithActionResult<RequestOffResponseDTO>
{
  private readonly IDayOffRequestService _dayOffRequestService;

  public GetRequestOffsEndPoints(IDayOffRequestService dayOffRequestService)
  {
    _dayOffRequestService = dayOffRequestService;
  }

  [HttpGet("calendar/requestOff")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [HasClaim("time_of_request")]
  public override async Task<ActionResult<RequestOffResponseDTO>> HandleAsync([FromQuery] RequestOffRequestDTO socialWallGetRequest, CancellationToken cancellationToken = default)
  {
    var postGetResponse = await _dayOffRequestService.GetRequestOffs(socialWallGetRequest);
    return Ok(postGetResponse);
  }
}
