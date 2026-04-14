using Asp.Versioning;
using DeviceManagerBE.Application.Features.Auth.Login;
using DeviceManagerBE.Contracts.Auth;
using DeviceManagerBE.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace DeviceManagerBE.Controllers;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [HttpPost("/api/auth/login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return this.ToProblem(
                StatusCodes.Status400BadRequest,
                "Validation Failed",
                "Du lieu dang nhap khong hop le.",
                "AUTH_VALIDATION_ERROR");
        }

        var result = await _mediator.Send(new LoginCommand
        {
            Username = request.Username,
            Password = request.Password,
            RememberMe = request.RememberMe
        }, cancellationToken);

        if (result.Success)
        {
            return Ok(new LoginResponse
            {
                Success = true,
                Message = result.Message,
                FullName = result.FullName,
                AccessToken = result.AccessToken,
                ExpiresAtUtc = result.ExpiresAtUtc
            });
        }

        return this.ToProblem(
            StatusCodes.Status401Unauthorized,
            "Unauthorized",
            result.Message,
            result.ErrorCode ?? "AUTH_INVALID_CREDENTIALS");
    }
}


