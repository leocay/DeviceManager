using Asp.Versioning;
using DeviceManagerBE.Application.Features.Devices;
using DeviceManagerBE.Contracts.Device;
using DeviceManagerBE.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace DeviceManagerBE.Controllers;

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

    [HttpPost]
    [ProducesResponseType(typeof(CreateDeviceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateDeviceResponse>> CreateDevice(
        [FromBody] CreateDeviceRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateDeviceCommand
        {
            DeviceCode = request.DeviceCode,
            DeviceName = request.DeviceName,
            CategoryId = request.CategoryId,
            Brand = request.Brand,
            Model = request.Model,
            SerialNumber = request.SerialNumber,
            PurchaseDate = request.PurchaseDate,
            WarrantyExpiryDate = request.WarrantyExpiryDate,
            Status = request.Status,
            EmployeeId = request.EmployeeId,
            Note = request.Note
        };

        var result = await _mediator.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new CreateDeviceResponse
        {
            DeviceId = result.DeviceId,
            DeviceCode = result.DeviceCode,
            DeviceName = result.DeviceName,
            Message = "Tao moi thiet bi thanh cong."
        });
    }
}


