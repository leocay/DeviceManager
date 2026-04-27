using DeviceManagerBE.Application.DTOs.Device;
using DeviceManagerBE.Application.Services.Device;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, CreateDeviceResultDto>
{
    private readonly IDeviceWriteRepository _deviceWriteRepository;

    public UpdateDeviceCommandHandler(IDeviceWriteRepository deviceWriteRepository)
    {
        _deviceWriteRepository = deviceWriteRepository;
    }

    public async Task<CreateDeviceResultDto> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _deviceWriteRepository.GetByIdAsync(request.DeviceId, cancellationToken)
            ?? throw new InvalidOperationException($"Device {request.DeviceId} not found.");

        device.DeviceCode = request.DeviceCode.Trim();
        device.DeviceName = request.DeviceName.Trim();
        device.CategoryId = request.CategoryId;
        device.Brand = string.IsNullOrWhiteSpace(request.Brand) ? null : request.Brand.Trim();
        device.Model = string.IsNullOrWhiteSpace(request.Model) ? null : request.Model.Trim();
        device.SerialNumber = string.IsNullOrWhiteSpace(request.SerialNumber) ? null : request.SerialNumber.Trim();
        device.PurchaseDate = request.PurchaseDate;
        device.WarrantyExpiryDate = request.WarrantyExpiryDate;
        device.Status = request.Status.Trim();
        device.EmployeeId = request.EmployeeId;
        device.Note = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim();
        device.UpdatedAt = DateTime.UtcNow;

        await _deviceWriteRepository.UpdateAsync(device, cancellationToken);

        return new CreateDeviceResultDto
        {
            DeviceId = device.DeviceId,
            DeviceCode = device.DeviceCode,
            DeviceName = device.DeviceName
        };
    }
}
