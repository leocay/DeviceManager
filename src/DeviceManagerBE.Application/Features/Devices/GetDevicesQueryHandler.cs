using DeviceManagerBE.Application.DTOs.Device;
using DeviceManagerBE.Application.Services.Device;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, GetDevicesResultDto>
{
    private readonly IDeviceReadRepository _deviceReadRepository;

    public GetDevicesQueryHandler(IDeviceReadRepository deviceReadRepository)
    {
        _deviceReadRepository = deviceReadRepository;
    }

    public async Task<GetDevicesResultDto> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        var result = await _deviceReadRepository.GetDevicesPaginatedAsync(
            request.SearchTerm,
            request.CategoryId,
            request.Status,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        return result;
    }
}

