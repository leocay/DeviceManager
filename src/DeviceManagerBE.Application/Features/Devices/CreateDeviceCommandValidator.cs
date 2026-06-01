using DeviceManagerBE.Application.Services.Device;
using FluentValidation;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class CreateDeviceCommandValidator : AbstractValidator<CreateDeviceCommand>
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

    public CreateDeviceCommandValidator(IDeviceWriteRepository deviceWriteRepository)
    {
        RuleFor(x => x.DeviceCode)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (deviceCode, cancellationToken) =>
                !string.IsNullOrWhiteSpace(deviceCode) &&
                !await deviceWriteRepository.DeviceCodeExistsAsync(deviceCode.Trim(), cancellationToken))
            .WithMessage("Ma thiet bi da ton tai.");

        RuleFor(x => x.DeviceName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .MustAsync((categoryId, cancellationToken) =>
                deviceWriteRepository.CategoryExistsAsync(categoryId, cancellationToken))
            .WithMessage("Loai thiet bi khong hop le.");

        RuleFor(x => x.Brand)
            .MaximumLength(100);

        RuleFor(x => x.Model)
            .MaximumLength(100);

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100);

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status =>
                !string.IsNullOrWhiteSpace(status) &&
                AllowedStatuses.Contains(status.Trim(), StringComparer.OrdinalIgnoreCase))
            .WithMessage("Trang thai khong hop le.");

        RuleFor(x => x.Note)
            .MaximumLength(500);

        RuleFor(x => x.EmployeeId)
            .MustAsync(async (employeeId, cancellationToken) =>
                !employeeId.HasValue || await deviceWriteRepository.EmployeeExistsAsync(employeeId.Value, cancellationToken))
            .WithMessage("Nhan vien su dung khong hop le.");

        RuleFor(x => x)
            .Must(x => !x.PurchaseDate.HasValue || !x.WarrantyExpiryDate.HasValue || x.WarrantyExpiryDate.Value.Date >= x.PurchaseDate.Value.Date)
            .WithMessage("Han bao hanh phai lon hon hoac bang ngay mua.");
    }
}
