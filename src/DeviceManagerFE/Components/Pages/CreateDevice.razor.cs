using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Features.Devices.Presentation.Services;
using DeviceManagerFE.Features.Devices.Presentation.ViewModels;
using DeviceManagerFE.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DeviceManagerFE.Components.Pages;

public class CreateDevicePageBase : ComponentBase
{
    [Inject] protected ICreateDeviceUseCase CreateDeviceUseCase { get; set; } = default!;
    [Inject] protected IUpdateDeviceUseCase UpdateDeviceUseCase { get; set; } = default!;
    [Inject] protected IGetDeviceDetailUseCase GetDeviceDetailUseCase { get; set; } = default!;
    [Inject] protected IGetEmployeeDirectoryUseCase GetEmployeeDirectoryUseCase { get; set; } = default!;
    [Inject] protected IDeviceInventoryPresenter DeviceInventoryPresenter { get; set; } = default!;
    [Inject] protected AuthSessionState AuthSessionState { get; set; } = default!;
    [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public int? DeviceId { get; set; }

    [SupplyParameterFromForm(FormName = "CreateDeviceForm")]
    protected CreateDeviceFormViewModel Model { get; set; } = new();

    protected EditContext EditContext { get; private set; } = default!;
    protected ValidationMessageStore ValidationMessageStore { get; private set; } = default!;
    protected bool Submitting { get; private set; }
    protected bool EmployeesLoading { get; private set; }
    protected bool PageLoading { get; private set; }
    protected string? StatusMessage { get; private set; }
    protected bool IsError { get; private set; }
    protected bool IsEditMode => DeviceId.HasValue;
    protected string PageHeading => IsEditMode ? "Chỉnh sửa thiết bị" : "Thêm thiết bị mới";
    protected string PageDescription => IsEditMode
        ? "Cập nhật thông tin thiết bị trong hệ thống quản lý tập trung."
        : "Nhập chi tiết thông tin thiết bị để đưa vào hệ thống quản lý tập trung.";
    protected string SaveButtonText => IsEditMode ? "Lưu thay đổi" : "Lưu thiết bị";
    protected string BrowserPageTitle => PageHeading;
    protected IReadOnlyList<FilterOptionViewModel> DeviceCategoryOptions => DeviceInventoryPresenter.CategoryOptions.Where(option => !string.IsNullOrWhiteSpace(option.Value)).ToList();
    protected IReadOnlyList<FilterOptionViewModel> DeviceStatusOptions => DeviceInventoryPresenter.StatusOptions.Where(option => !string.IsNullOrWhiteSpace(option.Value)).ToList();
    protected IReadOnlyList<EmployeeOptionDto> EmployeeOptions { get; private set; } = [];
    protected string DisplayName => string.IsNullOrWhiteSpace(AuthSessionState.FullName) ? "Administrator" : AuthSessionState.FullName;
    protected string AvatarText => string.IsNullOrWhiteSpace(DisplayName) ? "A" : DisplayName[..1].ToUpperInvariant();

    protected override async Task OnInitializedAsync()
    {
        if (!AuthSessionState.IsAuthenticated)
        {
            try
            {
                var token = await JSRuntime.InvokeAsync<string?>("blazorGetLocal", "__Auth_AccessToken");
                var name = await JSRuntime.InvokeAsync<string?>("blazorGetLocal", "__Auth_FullName");
                if (string.IsNullOrWhiteSpace(token))
                {
                    token = await JSRuntime.InvokeAsync<string?>("blazorGetCookie", "__Auth_AccessToken");
                    name = await JSRuntime.InvokeAsync<string?>("blazorGetCookie", "__Auth_FullName");
                }

                if (!string.IsNullOrWhiteSpace(token))
                {
                    AuthSessionState.AccessToken = token;
                    AuthSessionState.FullName = name;
                }
            }
            catch
            {
            }

            if (!AuthSessionState.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/");
                return;
            }
        }

        EmployeesLoading = true;
        PageLoading = true;

        try
        {
            EmployeeOptions = await GetEmployeeDirectoryUseCase.ExecuteAsync();

            if (IsEditMode)
            {
                var detail = await GetDeviceDetailUseCase.ExecuteAsync(DeviceId!.Value);
                if (detail is null)
                {
                    IsError = true;
                    StatusMessage = "Không tải được thông tin thiết bị.";
                    NavigationManager.NavigateTo("/devices");
                    return;
                }

                Model = MapToViewModel(detail);
            }

            EditContext = new EditContext(Model);
            ValidationMessageStore = new ValidationMessageStore(EditContext);
            EditContext.OnValidationRequested += (_, _) => ValidationMessageStore.Clear();
            EditContext.OnFieldChanged += (_, _) => ClearStatus();
        }
        finally
        {
            EmployeesLoading = false;
            PageLoading = false;
        }
    }

    protected async Task HandleSubmitAsync()
    {
        Submitting = true;
        ClearStatus();
        ValidationMessageStore.Clear();

        var request = new CreateDeviceRequestDto(
            Model.DeviceCode,
            Model.DeviceName,
            Model.CategoryId ?? 0,
            Model.Brand,
            Model.Model,
            Model.SerialNumber,
            Model.PurchaseDate,
            Model.WarrantyExpiryDate,
            Model.Status,
            Model.EmployeeId,
            Model.Note);

        var result = IsEditMode
            ? await UpdateDeviceUseCase.ExecuteAsync(DeviceId!.Value, request)
            : await CreateDeviceUseCase.ExecuteAsync(request);

        if (!result.Success)
        {
            IsError = true;
            StatusMessage = result.Message;
            ApplyServerValidation(result.ValidationErrors);
            Submitting = false;
            return;
        }

        NavigationManager.NavigateTo("/devices", forceLoad: false);
    }

    protected void Cancel()
    {
        NavigationManager.NavigateTo("/devices");
    }

    private static CreateDeviceFormViewModel MapToViewModel(DeviceEditorDto detail) => new()
    {
        DeviceCode = detail.DeviceCode,
        DeviceName = detail.DeviceName,
        CategoryId = detail.CategoryId,
        Brand = detail.Brand,
        Model = detail.Model,
        SerialNumber = detail.SerialNumber,
        PurchaseDate = detail.PurchaseDate,
        WarrantyExpiryDate = detail.WarrantyExpiryDate,
        Status = detail.Status,
        EmployeeId = detail.EmployeeId,
        Note = detail.Note
    };

    private void ApplyServerValidation(IReadOnlyDictionary<string, string[]> errors)
    {
        foreach (var error in errors)
        {
            if (string.IsNullOrWhiteSpace(error.Key))
            {
                continue;
            }

            var fieldName = error.Key switch
            {
                nameof(CreateDeviceRequestDto.CategoryId) => nameof(CreateDeviceFormViewModel.CategoryId),
                nameof(CreateDeviceRequestDto.EmployeeId) => nameof(CreateDeviceFormViewModel.EmployeeId),
                _ => error.Key
            };

            ValidationMessageStore.Add(new FieldIdentifier(Model, fieldName), error.Value);
        }

        EditContext.NotifyValidationStateChanged();
    }

    private void ClearStatus()
    {
        StatusMessage = null;
        IsError = false;
    }
}
