using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkReceiptAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkReceiptEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateEndPoint : EndpointBaseAsync.WithRequest<GenerateWorkReceiptRequest>.WithActionResult<WorkReceiptResponse>
{
  private readonly IWorkReceiptService _workReceiptService;

  public CreateEndPoint(IWorkReceiptService workReceiptService)
  {
    _workReceiptService = workReceiptService;
  }

  [HttpPost("calendar/generateworkreceipt")]
  public override async Task<ActionResult<WorkReceiptResponse>> HandleAsync(GenerateWorkReceiptRequest receiptRequest, CancellationToken cancellationToken = default)
  {
    if (receiptRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(receiptRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<WorkReceiptResponse>.Invalid(errors));
    }

    var result = await _workReceiptService.GenerateWorkReceipt(receiptRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
