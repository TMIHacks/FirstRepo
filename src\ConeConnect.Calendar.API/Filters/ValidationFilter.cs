using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConeConnect.Calendar.API.Filters;

public class ValidationFilter
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid)
    {
      context.Result = new UnprocessableEntityObjectResult(context.ModelState);
    }
  }
  public void OnActionExecuted(ActionExecutedContext context)
  {
  }
}
