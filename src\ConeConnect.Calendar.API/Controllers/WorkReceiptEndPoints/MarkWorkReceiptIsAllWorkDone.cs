using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.Services;
using ConeConnect.User.API.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers.WorkReceiptEndPoints; 
public class MarkWorkReceiptIsAllWorkDone : EndpointBase
{
  private readonly IWorkReceiptService _workReceiptService;

  public MarkWorkReceiptIsAllWorkDone(IWorkReceiptService workReceiptService)
  {
    _workReceiptService = workReceiptService;
  }

  [HttpPost("Calendar/ExecuteMarkWorkReceiptIsAllWorkDone")]
  [HeaderFilterAttribute()]
  public async Task<ActionResult<bool>> HandleAsync()
  {
    Task.Run(() =>
    {
      _workReceiptService.ExecuteMarkWorkReceiptIsAllWorkDone();
    });
    return new OkObjectResult(true);
  }

}
