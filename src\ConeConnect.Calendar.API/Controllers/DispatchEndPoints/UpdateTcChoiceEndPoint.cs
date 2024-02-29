using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;

namespace ConeConnect.Calendar.API.Controllers.DispatchEndPoints;
public class UpdateTcChoiceEndPoint : EndpointBase
{
  private readonly IDispatchService _dispatchService;
  private readonly IUserDetailService _userDetailService;

  public UpdateTcChoiceEndPoint(IDispatchService dispatchService, IUserDetailService userDetailService)
  {
    _dispatchService = dispatchService;
    _userDetailService = userDetailService;
  }

  [HttpPut("Calendar/UpdateTcChoice")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> HandleAsync(TcChoiceRQ tcChoiceRQ)
  {
    var data = await _dispatchService.UpdateTcChoice(tcChoiceRQ);
    return new OkObjectResult(data.GetValue());
  }

  [HttpPut("Calendar/UpdateTcWorkStatus")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> HandleAsync(AddTCWorkStatus tCWorkStatus)
  {
    var data = await _dispatchService.UpdateTcWorkStatus(tCWorkStatus);
    return new OkObjectResult(data.GetValue());
  }

  [HttpPut("Calendar/UpdateDriver")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> HandleAsync(UpdateDriverStatus updateDriverStatus)
  {
    var data = await _dispatchService.UpdateDriver(updateDriverStatus);
    return new OkObjectResult(data.GetValue());
  }
  [HttpGet("Calendar/GetAllBranch")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<List<RegionBranches>>> HandleAsync()
  {
    var branch = await _dispatchService.GetBranches();
    return Ok(branch.Value);
  }
  [HttpPut("Calendar/UpdateDispatchToIsRead")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<List<RegionBranches>>> HandleAsync(int dispatchNo)
  {
    var result = await _dispatchService.UpdateDispatchToIsRead(dispatchNo);
    return Ok(result.Value);
  }

  [HttpPut("Calendar/UpdateLead")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> HandleAsync(UpdateLeadStatusRequest updateLeadStatusRequest)
  {
    var data = await _dispatchService.UpdateLead(updateLeadStatusRequest);
    return new OkObjectResult(data.GetValue());
  }
  [HttpPut("Calendar/UpdateTruck")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<bool>> HandleAsync(UpdateTruckNumber updateTruckNumber)
  {
    var data = await _dispatchService.UpdateTruck(updateTruckNumber);
    return new OkObjectResult(data.GetValue());
  }
}
