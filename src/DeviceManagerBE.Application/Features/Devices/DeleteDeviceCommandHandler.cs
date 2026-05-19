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

    public Task<bool> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        => _deviceWriteRepository.DeleteAsync(request.DeviceId, cancellationToken);
}