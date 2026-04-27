using DeviceManagerBE.Application.DTOs.Device;
using DeviceManagerBE.Application.Services.Device;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, DeviceDetailDto?>
{
    private readonly IDeviceReadRepository _deviceReadRepository;

    public GetDeviceByIdQueryHandler(IDeviceReadRepository deviceReadRepository)
    {
        _deviceReadRepository = deviceReadRepository;
    }

    public Task<DeviceDetailDto?> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
        => _deviceReadRepository.GetByIdAsync(request.DeviceId, cancellationToken);
}
