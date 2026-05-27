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
                DeviceCode = d.DeviceCode,
                DeviceName = d.DeviceName,
                CategoryName = d.CategoryName,
                EmployeeName = d.EmployeeName,
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

    [HttpGet("{deviceId:int}")]
    [ProducesResponseType(typeof(GetDeviceDetailResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetDeviceDetailResponse>> GetDeviceById(
        int deviceId,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetDeviceByIdQuery(deviceId), cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(new GetDeviceDetailResponse
        {
            DeviceId = result.DeviceId,
            DeviceCode = result.DeviceCode,
            DeviceName = result.DeviceName,
            CategoryId = result.CategoryId,
            CategoryName = result.CategoryName,
            CategoryDescription = result.CategoryDescription,
            EmployeeId = result.EmployeeId,
            Brand = result.Brand,
            Model = result.Model,
            SerialNumber = result.SerialNumber,
            PurchaseDate = result.PurchaseDate,
            WarrantyExpiryDate = result.WarrantyExpiryDate,
            Status = result.Status,
            Note = result.Note,
            History = result.Logs
                .Select(log => new DeviceHistoryResponse
                {
                    ActionType = log.ActionType,
                    ActionBy = log.ActionBy,
                    ActionTime = log.ActionTime,
                    Content = log.Content
                })
                .ToList()
        });
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

    [HttpPut("{deviceId:int}")]
    [ProducesResponseType(typeof(UpdateDeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateDeviceResponse>> UpdateDevice(
        int deviceId,
        [FromBody] UpdateDeviceRequest request,
        CancellationToken cancellationToken = default)
    {
        var existing = await _mediator.Send(new GetDeviceByIdQuery(deviceId), cancellationToken);
        if (existing is null)
        {
            return NotFound();
        }

        var command = new UpdateDeviceCommand
        {
            DeviceId = deviceId,
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

        return Ok(new UpdateDeviceResponse
        {
            DeviceId = result.DeviceId,
            DeviceCode = result.DeviceCode,
            DeviceName = result.DeviceName,
            Message = "Cap nhat thiet bi thanh cong."
        });
    }

    [HttpDelete("{deviceId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDevice(int deviceId, CancellationToken cancellationToken = default)
    {
        var deleted = await _mediator.Send(new DeleteDeviceCommand { DeviceId = deviceId }, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}


