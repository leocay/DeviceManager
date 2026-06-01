using DeviceManagerBE.Application.Services.Device;
using FluentValidation;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    private static readonly string[] AllowedStatuses =
    [
        "Available",
        "In Stock",
        "In Use",
        "Maintenance",
        "Reserved",
        "Retired"
    ];

    public UpdateDeviceCommandValidator(IDeviceWriteRepository deviceWriteRepository)
    {
        RuleFor(x => x.DeviceId)
            .GreaterThan(0)
            .MustAsync(async (deviceId, cancellationToken) =>
                await deviceWriteRepository.GetByIdAsync(deviceId, cancellationToken) is not null)
            .WithMessage("Thiết bị không tồn tại.");

        RuleFor(x => x.DeviceCode)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (command, deviceCode, cancellationToken) =>
                !string.IsNullOrWhiteSpace(deviceCode) &&
                !await deviceWriteRepository.DeviceCodeExistsExceptAsync(deviceCode.Trim(), command.DeviceId, cancellationToken))
            .WithMessage("Mã thiết bị đã tồn tại.");

        RuleFor(x => x.DeviceName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .MustAsync((categoryId, cancellationToken) =>
                deviceWriteRepository.CategoryExistsAsync(categoryId, cancellationToken))
            .WithMessage("Loại thiết bị không hợp lệ.");

        RuleFor(x => x.Brand).MaximumLength(100);
        RuleFor(x => x.Model).MaximumLength(100);
        RuleFor(x => x.SerialNumber).MaximumLength(100);

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status =>
                !string.IsNullOrWhiteSpace(status) &&
                AllowedStatuses.Contains(status.Trim(), StringComparer.OrdinalIgnoreCase))
            .WithMessage("Trạng thái không hợp lệ.");

        RuleFor(x => x.Note).MaximumLength(500);

        RuleFor(x => x.EmployeeId)
            .MustAsync(async (employeeId, cancellationToken) =>
                !employeeId.HasValue || await deviceWriteRepository.EmployeeExistsAsync(employeeId.Value, cancellationToken))
            .WithMessage("Nhân viên sử dụng không hợp lệ.");

        RuleFor(x => x)
            .Must(x => !x.PurchaseDate.HasValue || !x.WarrantyExpiryDate.HasValue || x.WarrantyExpiryDate.Value.Date >= x.PurchaseDate.Value.Date)
            .WithMessage("Hạn bảo hành phải lớn hơn hoặc bằng ngày mua.");
    }
}
