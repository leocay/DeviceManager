using DeviceManagerBE.Application.DTOs.Device;
using DeviceManagerBE.Application.Services.Device;
using DeviceManagerBE.Application.Services.Employee;
using MediatR;

namespace DeviceManagerBE.Application.Features.Devices;

public sealed class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, CreateDeviceResultDto>
{
    private readonly IDeviceWriteRepository _deviceWriteRepository;
    private readonly IEmployeeReadRepository _employeeReadRepository;

    public UpdateDeviceCommandHandler(
        IDeviceWriteRepository deviceWriteRepository,
        IEmployeeReadRepository employeeReadRepository)
    {
        _deviceWriteRepository = deviceWriteRepository;
        _employeeReadRepository = employeeReadRepository;
    }

    public async Task<CreateDeviceResultDto> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await _deviceWriteRepository.GetByIdAsync(request.DeviceId, cancellationToken)
            ?? throw new InvalidOperationException($"Device {request.DeviceId} not found.");

        var previousDeviceCode = device.DeviceCode;
        var previousDeviceName = device.DeviceName;
        var previousStatus = device.Status;
        var previousEmployeeId = device.EmployeeId;
        var previousCategoryId = device.CategoryId;
        var previousBrand = device.Brand;
        var previousModel = device.Model;
        var previousSerialNumber = device.SerialNumber;
        var previousPurchaseDate = device.PurchaseDate;
        var previousWarrantyExpiryDate = device.WarrantyExpiryDate;
        var previousNote = device.Note;

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

        var employees = await _employeeReadRepository.GetEmployeesAsync(cancellationToken);
        var employeeNames = employees.ToDictionary(e => e.EmployeeId, e => e.FullName);

        var updateDetails = BuildUpdateDetails(
            previousDeviceCode,
            previousDeviceName,
            previousStatus,
            previousEmployeeId,
            previousCategoryId,
            previousBrand,
            previousModel,
            previousSerialNumber,
            previousPurchaseDate,
            previousWarrantyExpiryDate,
            previousNote,
            device,
            employeeNames);

        await _deviceWriteRepository.AddLogAsync(
            device.DeviceId,
            "Update",
            "Hệ thống Admin",
            updateDetails,
            cancellationToken);

        return new CreateDeviceResultDto
        {
            DeviceId = device.DeviceId,
            DeviceCode = device.DeviceCode,
            DeviceName = device.DeviceName
        };
    }

    private static string BuildUpdateDetails(
        string previousDeviceCode,
        string previousDeviceName,
        string previousStatus,
        int? previousEmployeeId,
        int previousCategoryId,
        string? previousBrand,
        string? previousModel,
        string? previousSerialNumber,
        DateTime? previousPurchaseDate,
        DateTime? previousWarrantyExpiryDate,
        string? previousNote,
        DeviceManagerBE.Domain.Entities.Device current,
        IReadOnlyDictionary<int, string> employeeNames)
    {
        var changes = new List<string>();

        if (!string.Equals(previousDeviceCode, current.DeviceCode, StringComparison.Ordinal))
        {
            changes.Add($"Mã: '{previousDeviceCode}' -> '{current.DeviceCode}'");
        }

        if (!string.Equals(previousDeviceName, current.DeviceName, StringComparison.Ordinal))
        {
            changes.Add($"Tên: '{previousDeviceName}' -> '{current.DeviceName}'");
        }

        if (previousCategoryId != current.CategoryId)
        {
            changes.Add($"Loại: {previousCategoryId} -> {current.CategoryId}");
        }

        if (!string.Equals(previousStatus, current.Status, StringComparison.Ordinal))
        {
            changes.Add($"Trạng thái: '{previousStatus}' -> '{current.Status}'");
        }

        if (!string.Equals(current.Brand ?? string.Empty, previousDeviceName /* placeholder */ ?? string.Empty, StringComparison.Ordinal))
        {
            // Brand isn't compared previously; include if present on current vs previous stored in device before change
        }

        if (previousEmployeeId != current.EmployeeId)
        {
            string prevName = previousEmployeeId.HasValue && employeeNames.TryGetValue(previousEmployeeId.Value, out var p) ? p : previousEmployeeId?.ToString() ?? "(trống)";
            string currName = current.EmployeeId.HasValue && employeeNames.TryGetValue(current.EmployeeId.Value, out var c) ? c : current.EmployeeId?.ToString() ?? "(trống)";
            changes.Add($"Nhân viên: '{prevName}' -> '{currName}'");
        }

        if (!string.Equals(previousBrand ?? string.Empty, current.Brand ?? string.Empty, StringComparison.Ordinal))
        {
            changes.Add($"Hãng: '{previousBrand ?? "(trống)"}' -> '{current.Brand ?? "(trống)"}'");
        }

        if (!string.Equals(previousModel ?? string.Empty, current.Model ?? string.Empty, StringComparison.Ordinal))
        {
            changes.Add($"Model: '{previousModel ?? "(trống)"}' -> '{current.Model ?? "(trống)"}'");
        }

        if (!string.Equals(previousSerialNumber ?? string.Empty, current.SerialNumber ?? string.Empty, StringComparison.Ordinal))
        {
            changes.Add($"Số serial: '{previousSerialNumber ?? "(trống)"}' -> '{current.SerialNumber ?? "(trống)"}'");
        }

        if (previousPurchaseDate != current.PurchaseDate)
        {
            var prev = previousPurchaseDate?.ToString("dd/MM/yyyy") ?? "(trống)";
            var curr = current.PurchaseDate?.ToString("dd/MM/yyyy") ?? "(trống)";
            changes.Add($"Ngày mua: '{prev}' -> '{curr}'");
        }

        if (previousWarrantyExpiryDate != current.WarrantyExpiryDate)
        {
            var prev = previousWarrantyExpiryDate?.ToString("dd/MM/yyyy") ?? "(trống)";
            var curr = current.WarrantyExpiryDate?.ToString("dd/MM/yyyy") ?? "(trống)";
            changes.Add($"Hạn bảo hành: '{prev}' -> '{curr}'");
        }

        if (!string.Equals(previousNote ?? string.Empty, current.Note ?? string.Empty, StringComparison.Ordinal))
        {
            changes.Add($"Ghi chú: '{previousNote ?? "(trống)"}' -> '{current.Note ?? "(trống)"}'");
        }

        return changes.Count == 0 ? string.Empty : string.Join("; ", changes);
    }
}
