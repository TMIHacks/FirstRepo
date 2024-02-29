using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.API.Helper;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class UpdateRequestOffStatusEndPoint : EndpointBaseAsync.WithRequest<BulkStatusUpdateRequestDTO>.WithActionResult<bool>
{
  private readonly IDayOffRequestService _dayOffRequestService;
  private readonly IUserDetailService _userDetailService;

  public UpdateRequestOffStatusEndPoint(IDayOffRequestService dayOffRequestService,
    IUserDetailService userDetailService)
  {
    _dayOffRequestService = dayOffRequestService;
    _userDetailService = userDetailService;
  }

  [HttpPut("calendar/requestOff/status")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  [HasClaim("time_of_request")]
  public override async Task<ActionResult<bool>> HandleAsync(BulkStatusUpdateRequestDTO request, CancellationToken cancellationToken = default)
  {
    var userInfo = await _userDetailService.GetUser(_userDetailService.userID);
    return await _dayOffRequestService.UpdateRequestOffsStatus(request, userInfo);
  }
}
