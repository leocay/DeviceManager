using DeviceManagerBE.Application.DTOs.Device;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class GetDevicesQuery : IRequest<GetDevicesResultDto>
{
    public string? SearchTerm { get; init; }
    public string? CategoryId { get; init; }
    public string? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

