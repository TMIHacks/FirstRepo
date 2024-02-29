using Ardalis.ApiEndpoints;
using Ardalis.Result;
using ConeConnect.Calendar.Core.DispatchAggregate;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.Services;
using ConeConnect.Calendar.Core.WorkReceiptAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkReceiptEndPoints;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CreateCallOutEndPoint : EndpointBaseAsync.WithRequest<EmployeeCallOutIncidentsRequest>.WithActionResult<EmployeeCallOutIncidentsRequest>
{
  private readonly IWorkReceiptService _workReceiptService;

  public CreateCallOutEndPoint(IWorkReceiptService workReceiptService)
  {
    _workReceiptService = workReceiptService;
  }

  [HttpPost("calendar/CallOutIncidents")]
  public override async Task<ActionResult<EmployeeCallOutIncidentsRequest>> HandleAsync(EmployeeCallOutIncidentsRequest callOutRequest, CancellationToken cancellationToken = default)
  {
    EmployeeCallOutIncidentsRequest callOutReturnRequest = await _workReceiptService.EmployeeCallOutIncidents(callOutRequest);
    return (callOutReturnRequest);
  }
}
