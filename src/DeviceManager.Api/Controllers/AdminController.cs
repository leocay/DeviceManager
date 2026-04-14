using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeviceManager.Api.Controllers;

[ApiController]
[Authorize]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/admin")]
public class AdminController : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public IActionResult Me()
    {
        return Ok(new
        {
            AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Username = User.FindFirstValue(ClaimTypes.Name),
            Role = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}