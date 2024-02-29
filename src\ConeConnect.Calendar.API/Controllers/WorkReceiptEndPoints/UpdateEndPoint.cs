using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkReceiptAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkReceiptEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UpdateEndPoint : EndpointBaseAsync.WithRequest<UpdateWorkReceiptRequest>.WithActionResult<WorkReceiptResponse>
{
  private readonly IWorkReceiptService _workReceiptService;

  public UpdateEndPoint(IWorkReceiptService workReceiptService)
  {
    _workReceiptService = workReceiptService;
  }

  [HttpPut("calendar/workreceipt/{dispatchNo}/{id}")]
  public override async Task<ActionResult<WorkReceiptResponse>> HandleAsync(UpdateWorkReceiptRequest receiptRequest, CancellationToken cancellationToken = default)  {
    if (receiptRequest == null)
    {
      var errors = new List<ValidationError>
      {
        new() { Identifier = nameof(receiptRequest), ErrorMessage = $"Invalid Request Data" }
      };

      return BadRequest(Result<WorkReceiptResponse>.Invalid(errors));
    }

    var result = await _workReceiptService.UpdateWorkReceipt(receiptRequest);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
