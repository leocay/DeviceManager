using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Features.Devices.Presentation.Services;
using DeviceManagerFE.Features.Devices.Presentation.ViewModels;
using DeviceManagerFE.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DeviceManagerFE.Components.Pages;

public class CreateDevicePageBase : ComponentBase
{
    [Inject] protected ICreateDeviceUseCase CreateDeviceUseCase { get; set; } = default!;
    [Inject] protected IGetEmployeeDirectoryUseCase GetEmployeeDirectoryUseCase { get; set; } = default!;
    [Inject] protected IDeviceInventoryPresenter DeviceInventoryPresenter { get; set; } = default!;
    [Inject] protected AuthSessionState AuthSessionState { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    [SupplyParameterFromForm(FormName = "CreateDeviceForm")]
    protected CreateDeviceFormViewModel Model { get; set; } = new();

    protected EditContext EditContext { get; private set; } = default!;
    protected ValidationMessageStore ValidationMessageStore { get; private set; } = default!;
    protected bool Submitting { get; private set; }
    protected bool EmployeesLoading { get; private set; }
    protected string? StatusMessage { get; private set; }
    protected bool IsError { get; private set; }
    protected IReadOnlyList<FilterOptionViewModel> DeviceCategoryOptions => DeviceInventoryPresenter.CategoryOptions.Where(option => !string.IsNullOrWhiteSpace(option.Value)).ToList();
    protected IReadOnlyList<FilterOptionViewModel> DeviceStatusOptions => DeviceInventoryPresenter.StatusOptions.Where(option => !string.IsNullOrWhiteSpace(option.Value)).ToList();
    protected IReadOnlyList<EmployeeOptionDto> EmployeeOptions { get; private set; } = [];
    protected string DisplayName => string.IsNullOrWhiteSpace(AuthSessionState.FullName) ? "Administrator" : AuthSessionState.FullName;
    protected string AvatarText => string.IsNullOrWhiteSpace(DisplayName) ? "A" : DisplayName[..1].ToUpperInvariant();

    protected override async Task OnInitializedAsync()
    {
        if (!AuthSessionState.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        EditContext = new EditContext(Model);
        ValidationMessageStore = new ValidationMessageStore(EditContext);
        EditContext.OnValidationRequested += (_, _) => ValidationMessageStore.Clear();
        EditContext.OnFieldChanged += (_, _) => ClearStatus();

        EmployeesLoading = true;
        try
        {
            EmployeeOptions = await GetEmployeeDirectoryUseCase.ExecuteAsync();
        }
        finally
        {
            EmployeesLoading = false;
        }
    }

    protected async Task HandleSubmitAsync()
    {
        Submitting = true;
        ClearStatus();
        ValidationMessageStore.Clear();

        var result = await CreateDeviceUseCase.ExecuteAsync(new CreateDeviceRequestDto(
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
            Model.Note));

        if (!result.Success)
        {
            IsError = true;
            StatusMessage = result.Message;
            ApplyServerValidation(result.ValidationErrors);
            Submitting = false;
            return;
        }

        IsError = false;
        StatusMessage = result.Message;
        NavigationManager.NavigateTo("/devices", forceLoad: false);
    }

    protected void Cancel()
    {
        NavigationManager.NavigateTo("/devices");
    }

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
