using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.WorkReceiptAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkReceiptEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class GetByIdEndPoint : EndpointBaseAsync.WithRequest<GetWorkReceiptByIdRequest>.WithActionResult<WorkReceiptResponse>
{
  private readonly IWorkReceiptService _workReceiptService;

  public GetByIdEndPoint(IWorkReceiptService workReceiptService)
  {
    _workReceiptService = workReceiptService;
  }

  [HttpGet("calendar/workreceipt/{dispatchNo}/{id}")]
  public override async Task<ActionResult<WorkReceiptResponse>> HandleAsync([FromRoute] GetWorkReceiptByIdRequest request, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrEmpty(request.id) || request.dispatchNo == null || request.dispatchNo == 0)
    {
      var errors = new List<ValidationError>
      {
        new() { ErrorMessage = $"Invalid params" }
      };

      return BadRequest(Result<WorkReceiptResponse>.Invalid(errors));
    }

    var result = await _workReceiptService.GetWorkReceipt(request.dispatchNo, request.id);

    if (result.Status == ResultStatus.NotFound)
      return Ok(new { });

    if (result.Status == ResultStatus.Invalid)
      return BadRequest(result.ValidationErrors);

    return new OkObjectResult(result.GetValue());
  }
}
