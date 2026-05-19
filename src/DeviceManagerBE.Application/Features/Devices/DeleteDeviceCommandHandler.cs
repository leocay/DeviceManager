using DeviceManagerBE.Application.Services.Device;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, bool>
{
    private readonly IDeviceWriteRepository _deviceWriteRepository;

    public DeleteDeviceCommandHandler(IDeviceWriteRepository deviceWriteRepository)
    {
        _deviceWriteRepository = deviceWriteRepository;
    }

    public async Task<bool> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _deviceWriteRepository.GetByIdAsync(request.DeviceId, cancellationToken);
        if (device is null)
        {
            return false;
        }

        if (string.Equals(device.Status, "Deleted", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        await _deviceWriteRepository.AddLogAsync(
            request.DeviceId,
            "Delete",
            "Hệ thống Admin",
            $"Xóa thiết bị '{device.DeviceCode}'.",
            cancellationToken);

        return await _deviceWriteRepository.DeleteAsync(request.DeviceId, cancellationToken);
    }
}