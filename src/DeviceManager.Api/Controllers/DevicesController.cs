using Asp.Versioning;
using DeviceManager.Application.Features.Devices;
using DeviceManager.Contracts.Device;
using DeviceManager.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace DeviceManager.Api.Controllers;

[ApiController]
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/devices")]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DevicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetDevicesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetDevicesResponse>> GetDevices(
        [FromQuery] string? searchTerm,
        [FromQuery] string? categoryId,
        [FromQuery] string? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Validate pagination
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return this.ToProblem(
                StatusCodes.Status400BadRequest,
                "Invalid Pagination",
                "Trang hoac so luong phai lon hon 0, va so luong khong duoc vuot qua 100.",
                "PAGINATION_INVALID");
        }

        var query = new GetDevicesQuery
        {
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            Status = status,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query, cancellationToken);

        var response = new GetDevicesResponse
        {
            Devices = result.Devices.Select(d => new DeviceDto
            {
                DeviceId = d.DeviceId,
                DeviceName = d.DeviceName,
                CategoryName = d.CategoryName,
                SerialNumber = d.SerialNumber,
                Status = d.Status,
                Brand = d.Brand,
                Model = d.Model
            }).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages,
            HasPreviousPage = result.HasPreviousPage,
            HasNextPage = result.HasNextPage
        };

        return Ok(response);
    }
}
