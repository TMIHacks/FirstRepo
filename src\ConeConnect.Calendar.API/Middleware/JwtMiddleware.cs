
using ConeConnect.Calendar.Core.Interfaces;
using FirebaseAdmin.Auth;

namespace ConeConnect.Calendar.API.Middleware;

public class JwtMiddleware
{
  private readonly RequestDelegate _next;

  public JwtMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context, IUserDetailService userDetailService)
  {
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

    if (token != null)
    {
      FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
      string uid = decodedToken.Uid;

      object userID;

      if (decodedToken.Claims.TryGetValue("UserId", out userID))
      {
        userDetailService.userID = Convert.ToString(userID);
      }

      object displayName;

      if (decodedToken.Claims.TryGetValue("DisplayName", out displayName))
      {
        userDetailService.UserName = Convert.ToString(displayName);
      } 

      object EmpId;
      if (decodedToken.Claims.TryGetValue("EmpId", out EmpId))
      {
        userDetailService.EmpId = Convert.ToInt32(EmpId);
      }
    }

    await _next(context);
  }
}
