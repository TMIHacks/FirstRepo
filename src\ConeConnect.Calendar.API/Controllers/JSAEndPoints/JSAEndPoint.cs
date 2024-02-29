using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.JSAAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.JSAEndPoints; 
public class JSAEndPoint : EndpointBase
{
  private readonly IJSAService _jsaService;

  public JSAEndPoint(IJSAService jsaService)
  {
    _jsaService = jsaService;
  }

  [HttpPost("Calendar/AddUpdateJSA")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<JSARQRS>> HandleAsync(JSARQRS jsaRequest)
  {
    var data = await _jsaService.AddUpdateJSA(jsaRequest);
    return Ok(data.Value);
  }

  [HttpGet("Calendar/GetJSAByDispatchNo")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public async Task<ActionResult<List<JSARQRS>>> HandleAsync(int dispatchNo)
  {
    var data = await _jsaService.GetJSAByDispatchNo(dispatchNo);
    return Ok(data.Value);
  }
}
