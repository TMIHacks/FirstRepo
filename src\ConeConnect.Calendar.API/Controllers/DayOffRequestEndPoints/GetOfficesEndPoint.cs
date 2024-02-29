using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class GetOfficesEndPoint : EndpointBaseAsync.WithoutRequest
    .WithActionResult<List<OfficeResponseModel>>
{
  private readonly IDayOffRequestService _dayOffRequestService;
  public GetOfficesEndPoint(IDayOffRequestService dayOffRequestService)
  {
    _dayOffRequestService = dayOffRequestService;
  }

  [HttpGet("Calendar/offices")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public override async Task<ActionResult<List<OfficeResponseModel>>> HandleAsync(CancellationToken cancellationToken = default)
  {
    List<OfficeResponseModel> customers = await _dayOffRequestService.GetOffices();
    return Ok(customers);
  }
}
