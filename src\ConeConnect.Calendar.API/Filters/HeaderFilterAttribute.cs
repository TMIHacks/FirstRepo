using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; 
namespace ConeConnect.User.API.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class HeaderFilterAttribute : Attribute, IAuthorizationFilter
{
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    if (context.HttpContext.Request.Path.ToString().ToLower() == "/external/authcode/validate"
      || context.HttpContext.Request.Path.ToString().ToLower() == "/external/authcode/generate"
      )
    {
      var builder = WebApplication.CreateBuilder();
      var ValidateAuthKey = builder.Configuration.GetSection("ValidateAuthKey").Value;

      string headerAuthKey = context.HttpContext.Request.Headers["AuthKey"];
      if (headerAuthKey == null || headerAuthKey.ToLower() != ValidateAuthKey.ToLower())
      {
        context.HttpContext.Response.StatusCode = 400;
        context.Result = new JsonResult(new { message = "BadRequest" }) { StatusCode = StatusCodes.Status400BadRequest };
      }
    }
  }
}
