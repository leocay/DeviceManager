using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Features.Devices.Presentation.Services;
using DeviceManagerFE.Services;
using Microsoft.AspNetCore.Components;

namespace DeviceManagerFE.Components.Pages;

public class DeviceDetailPageBase : ComponentBase
{
    [Inject] protected IGetDeviceDetailUseCase GetDeviceDetailUseCase { get; set; } = default!;
    [Inject] protected IGetEmployeeDirectoryUseCase GetEmployeeDirectoryUseCase { get; set; } = default!;
    [Inject] protected IDeviceInventoryPresenter DeviceInventoryPresenter { get; set; } = default!;
    [Inject] protected AuthSessionState AuthSessionState { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public int DeviceId { get; set; }

    protected DeviceEditorDto? DeviceDetail { get; private set; }
    protected IReadOnlyList<EmployeeOptionDto> Employees { get; private set; } = [];
    protected bool PageLoading { get; private set; }
    protected string? StatusMessage { get; private set; }
    protected bool IsError { get; private set; }
    protected bool CanEdit => DeviceDetail is not null;
    protected string BrowserPageTitle => DeviceDetail is null
        ? "Thông tin thiết bị"
        : $"{DeviceDetail.DeviceCode} - Thông tin thiết bị";

    protected string DisplayName => string.IsNullOrWhiteSpace(AuthSessionState.FullName)
        ? "Administrator"
        : AuthSessionState.FullName;

    protected string AvatarText => string.IsNullOrWhiteSpace(DisplayName)
        ? "A"
        : DisplayName[..1].ToUpperInvariant();

    protected string CategoryName
    {
        get
        {
            if (DeviceDetail is null)
            {
                return "-";
            }

            var category = DeviceInventoryPresenter.CategoryOptions.FirstOrDefault(
                option => option.Value == DeviceDetail.CategoryId.ToString());

            return category?.Label ?? "-";
        }
    }

    protected string StatusLabel
    {
        get
        {
            if (DeviceDetail is null)
            {
                return "-";
            }

            var status = DeviceInventoryPresenter.StatusOptions.FirstOrDefault(
                option => string.Equals(option.Value, DeviceDetail.Status, StringComparison.OrdinalIgnoreCase));

            return status?.Label ?? DeviceDetail.Status;
        }
    }

    protected string StatusCssClass => StatusLabel switch
    {
        "Mới" => "is-new",
        "Đang sử dụng" => "is-active",
        "Bảo trì" => "is-maintenance",
        "Đã đặt trước" => "is-reserved",
        "Hỏng" => "is-retired",
        _ => "is-default"
    };

    protected string EmployeeName
    {
        get
        {
            if (DeviceDetail?.EmployeeId is not int employeeId)
            {
                return "Chưa phân công";
            }

            var employee = Employees.FirstOrDefault(item => item.EmployeeId == employeeId);
            if (employee is null)
            {
                return "Chưa xác định";
            }

            // Prefer FullName if available, otherwise fall back to DisplayName and strip code prefix if present
            if (!string.IsNullOrWhiteSpace(employee.FullName))
            {
                return employee.FullName;
            }

            var display = employee.DisplayName;
            if (string.IsNullOrWhiteSpace(display))
            {
                return "Chưa xác định";
            }

            // Strip leading code like "C024 - " if present
            var idx = display.IndexOf(" - ");
            return idx > 0 ? display.Substring(idx + 3).Trim() : display;
        }
    }

    protected string OwnerInitial => string.IsNullOrWhiteSpace(EmployeeName)
        ? "?"
        : EmployeeName[..1].ToUpperInvariant();

    protected string OwnerDescription => DeviceDetail?.EmployeeId is null
        ? "Thiết bị hiện chưa được cấp phát cho nhân viên."
        : "Nhân viên đang phụ trách vận hành thiết bị này.";

    protected string DisplayBrand => string.IsNullOrWhiteSpace(DeviceDetail?.Brand) ? "-" : DeviceDetail.Brand;
    protected string DisplayModel => string.IsNullOrWhiteSpace(DeviceDetail?.Model) ? "-" : DeviceDetail.Model;
    protected string DisplaySerialNumber => string.IsNullOrWhiteSpace(DeviceDetail?.SerialNumber) ? "-" : DeviceDetail.SerialNumber;
    protected string DisplayPurchaseDate => DeviceDetail?.PurchaseDate?.ToString("dd/MM/yyyy") ?? "-";
    protected string DisplayWarrantyDate => DeviceDetail?.WarrantyExpiryDate?.ToString("dd/MM/yyyy") ?? "-";
    protected string DisplayNote => string.IsNullOrWhiteSpace(DeviceDetail?.Note) ? "Không có ghi chú." : DeviceDetail.Note;

    protected IReadOnlyList<DeviceHistoryItem> HistoryItems
    {
        get
        {
            if (DeviceDetail is null)
            {
                return [new DeviceHistoryItem("-", "Chưa có dữ liệu lịch sử", "-")];
            }

            if (DeviceDetail.History.Count == 0)
            {
                return [new DeviceHistoryItem("-", "Chưa có dữ liệu lịch sử", "-")];
            }

            return DeviceDetail.History
                .OrderByDescending(log => log.ActionTime)
                .Select(log => new DeviceHistoryItem(
                    log.ActionTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm"),
                    ToActionLabel(log.ActionType),
                    ToChangeContent(log.Content)))
                .ToList();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (!AuthSessionState.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        PageLoading = true;
        StatusMessage = null;

        try
        {
            DeviceDetail = await GetDeviceDetailUseCase.ExecuteAsync(DeviceId);

            if (DeviceDetail is null)
            {
                IsError = true;
                StatusMessage = "Không tìm thấy thông tin thiết bị hoặc thiết bị đã bị xóa.";
                return;
            }

            try
            {
                Employees = await GetEmployeeDirectoryUseCase.ExecuteAsync();
            }
            catch
            {
                Employees = [];
            }
        }
        catch
        {
            IsError = true;
            StatusMessage = "Có lỗi khi tải thông tin thiết bị.";
        }
        finally
        {
            PageLoading = false;
        }
    }

    protected void BackToList()
        => NavigationManager.NavigateTo("/devices");

    protected void OpenEditPage()
    {
        if (DeviceDetail is null)
        {
            return;
        }

        NavigationManager.NavigateTo($"/devices/{DeviceDetail.DeviceId}/edit");
    }

    private static string ToActionLabel(string actionType)
    {
        return actionType.Trim().ToLowerInvariant() switch
        {
            "create" => "Tạo mới thiết bị",
            "update" => "Cập nhật thông tin",
            "delete" => "Xóa thiết bị",
            _ => string.IsNullOrWhiteSpace(actionType) ? "Thao tác hệ thống" : actionType
        };
    }

    private static string ToChangeContent(string? content)
        => string.IsNullOrWhiteSpace(content) ? "Không có chi tiết thay đổi." :
           // strip accidental leading prefixes like "Cập nhật thông tin thiết bị: " if present
           (content.StartsWith("Cập nhật thông tin thiết bị", StringComparison.Ordinal) ? content.Substring(content.IndexOf(':') + 1).Trim() : content);

    protected sealed record DeviceHistoryItem(
        string DateText,
        string Action,
        string ChangeContent);
}
