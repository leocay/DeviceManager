using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/system")]
public class SystemController : ControllerBase
{
    [HttpGet("/")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Root()
    {
        return Ok(new
        {
            Project = "DeviceManager",
            Status = "Running"
        });
    }

    [HttpGet("health")]
    [HttpGet("/api/v1/system/health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new
        {
            Status = "Healthy"
        });
    }
}
