using Ardalis.ApiEndpoints;
using ConeConnect.Calendar.Core.Interfaces;
using ConeConnect.Calendar.Core.UserAggregate;
using Microsoft.AspNetCore.Mvc;

namespace ConeConnect.Calendar.API.Controllers;
public class UserDetailsController : EndpointBaseAsync.WithRequest<string>.WithActionResult<string>
{

  private readonly IUserDetailService _userDetailService;

  public UserDetailsController(IUserDetailService userDetailService)
  {
    _userDetailService = userDetailService;
  }

  [HttpGet("User/UserDetail")]
  public override async Task<ActionResult<string>> HandleAsync([FromRoute] string id, CancellationToken cancellationToken = default)
  {
    var data = await _userDetailService.GetUserName(id);
    return Ok(data);
  }
}
