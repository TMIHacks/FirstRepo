using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkReceiptAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkReceiptEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetEndPoint : EndpointBaseAsync.WithRequest<long>.WithActionResult<List<WorkReceiptResponse>>
{
  private readonly IWorkReceiptService _workReceiptService;

  public GetEndPoint(IWorkReceiptService workReceiptService)
  {
    _workReceiptService = workReceiptService;
  }

  [HttpGet("calendar/workreceipts/{dispatchNo}")]
  public override async Task<ActionResult<List<WorkReceiptResponse>>> HandleAsync(long dispatchNo, CancellationToken cancellationToken = default)
  {
    if (dispatchNo == null || dispatchNo == 0)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(dispatchNo), ErrorMessage = $"Dispatch number is required" }
      };

      return BadRequest(Result<WorkReceiptResponse>.Invalid(errors));
    }

    var result = await _workReceiptService.GetWorkReceipts(dispatchNo);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new List<WorkReceiptResponse>());

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
