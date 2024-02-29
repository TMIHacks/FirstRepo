using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class AddDvirEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public AddDvirEndPoint(IDispatchService dispatchService
    , IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  }

  [HttpPost("Calendar/AddUpdateDVIR")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<DVIRDTO>> HandleAsync(DVIRDTO dvirRQ)
  {
    var dvir = await _dispatchService.AddUpdateDVIR(dvirRQ);
    return Ok(dvir.Value);
  }  

  [HttpGet("Calendar/GetDVIRByDispatchID")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<DVIRDTO>> HandleAsync(string dispatchNo, bool isPreTrip)
  {
    var post = await _dispatchService.GetDVIRById(dispatchNo, isPreTrip);
    return Ok(post.Value);
  }
  [HttpDelete("Calendar/DeleteDVIR")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<DVIRDTO>> HandleAsync(string dispatchNo, string vehicleNo)
  {
    var dvir = await _dispatchService.DeleteDVIR(dispatchNo,vehicleNo);
    return Ok(dvir.Value);
  }

  [HttpGet("Calendar/GetAllRegion")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<Region>> HandleAsync()
  {
    var region = await _dispatchService.GetAllRegion();
    return Ok(region.Value);
  }
  [HttpGet("Calendar/GetBranchByRegionId")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<RegionBranches>> HandleAsync(string RegionId)
  {
    var branch = await _dispatchService.GetBranchByRegionId(RegionId);
    return Ok(branch.Value);
  }

}
