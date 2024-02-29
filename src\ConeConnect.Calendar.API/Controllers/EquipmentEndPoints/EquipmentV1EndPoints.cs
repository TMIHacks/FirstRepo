using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.EquipmentEndPoints;

public class EquipmentV1EndPoints : EndpointBase
{
  private readonly IEquipmentService _equipmentService;

  public EquipmentV1EndPoints(IEquipmentService equipmentService)
  {
    _equipmentService = equipmentService;
  }

  [HttpPost("Calendar/AddUpdateEquipmentV1")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<EquipmenRS>> HandleAsync(EquipmenRS equipment)
  {
    var data = await _equipmentService.AddUpdateEquipmentV1(equipment);
    if (data.IsSuccess)
    {
      return new OkObjectResult(data.GetValue());
    }
    else
    {
      return BadRequest(new { data.Errors });
    }
  }

  [HttpGet("Calendar/GetEquipmentV1")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<EquipmenRS>> HandleAsync(int dispatchNo, string geoLat, string geoLon, bool isReturnPrevious)
  {
    var data = await _equipmentService.GetEquipmentByDispatchV1(dispatchNo, geoLat, geoLon, isReturnPrevious);
    if (data.IsSuccess)
    {
      return new OkObjectResult(data.GetValue());
    }
    else
    {
      return BadRequest(new { data.Errors });
    }
  }
}
