using System.Reflection.Metadata;
using Ardalis.ApiEndpoints;
using Azure.Core;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace ConeConnect.Calendar.API.Controllers.DocumentsEndPoints;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DocumentEndPoint  : EndpointBaseAsync.WithRequest<Core.DispatchAggregate.Document>.WithActionResult<List<ContentDTO>>
{
  private readonly IDispatchService _dispatchService;

  public DocumentEndPoint(IDispatchService dispatchService)
  {
    _dispatchService = dispatchService;
  }

  [HttpPost("Calendar/Documents"), DisableRequestSizeLimit]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
  public override async Task<ActionResult<List<ContentDTO>>> HandleAsync([FromForm] Core.DispatchAggregate.Document request, CancellationToken cancellationToken = default)
  {
    var document = await _dispatchService.UploadFile(request);
    return Ok(document);
  }
  [HttpPost("Calendar/DocumentsTest"), DisableRequestSizeLimit]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public  async Task<ActionResult<List<ContentDTO>>> HandleAsync([FromForm] Core.DispatchAggregate.Document request)
  {
    var document = await _dispatchService.UploadFileTest(request);
    return Ok(document);
  }
}
