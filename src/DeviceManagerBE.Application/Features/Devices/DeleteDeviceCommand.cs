using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class DeleteDeviceCommand : IRequest<bool>
{
    public int DeviceId { get; init; }
}