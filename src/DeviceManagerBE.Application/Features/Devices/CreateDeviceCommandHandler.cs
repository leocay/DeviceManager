using DeviceManagerBE.Application.DTOs.Device;
using DeviceManagerBE.Application.Services.Device;
using DeviceManagerBE.Domain.Entities;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, CreateDeviceResultDto>
{
    private readonly IDeviceWriteRepository _deviceWriteRepository;

    public CreateDeviceCommandHandler(IDeviceWriteRepository deviceWriteRepository)
    {
        _deviceWriteRepository = deviceWriteRepository;
    }

    public async Task<CreateDeviceResultDto> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = new Device
        {
            DeviceCode = request.DeviceCode.Trim(),
            DeviceName = request.DeviceName.Trim(),
            CategoryId = request.CategoryId,
            EmployeeId = request.EmployeeId,
            Brand = Normalize(request.Brand),
            Model = Normalize(request.Model),
            SerialNumber = Normalize(request.SerialNumber),
            PurchaseDate = request.PurchaseDate?.Date,
            WarrantyExpiryDate = request.WarrantyExpiryDate?.Date,
            Status = request.Status.Trim(),
            Quantity = 1,
            Note = Normalize(request.Note),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _deviceWriteRepository.AddAsync(device, cancellationToken);

        return new CreateDeviceResultDto
        {
            DeviceId = created.DeviceId,
            DeviceCode = created.DeviceCode,
            DeviceName = created.DeviceName
        };
    }

    private static string? Normalize(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
