using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace ConeConnect.Calendar.API.Helper;

public class HasClaimAttribute : Attribute, IActionFilter
{

  private readonly string[] _claims;

  public HasClaimAttribute(params string[] claims)
  {
    _claims = claims;
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
  }

  public void OnActionExecuting(ActionExecutingContext context)
  {
    var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    if (!string.IsNullOrWhiteSpace(token))
    {
      FirebaseToken decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token).Result;

      if (decodedToken != null && decodedToken.Claims.TryGetValue("AdminClaims", out var claimObject))
      {
        if (claimObject != null)
        {
          var arr = ((IEnumerable)claimObject).Cast<object>().Select(x => x.ToString()).ToArray();
          if (arr?.Length > 0 && arr.Any(c => _claims.Contains(c)))
          {
            return;
          }
        }
      }
    }
    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
  }
}
