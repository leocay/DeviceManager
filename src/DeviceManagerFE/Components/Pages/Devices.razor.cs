using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Features.Devices.Domain.ValueObjects;
using DeviceManagerFE.Features.Devices.Presentation.Services;
using DeviceManagerFE.Features.Devices.Presentation.ViewModels;
using DeviceManagerFE.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DeviceManagerFE.Components.Pages;

public class DevicesPageBase : ComponentBase
{
    [Inject] protected IGetDeviceInventoryUseCase GetDeviceInventoryUseCase { get; set; } = default!;
    [Inject] protected IDeviceInventoryPresenter DeviceInventoryPresenter { get; set; } = default!;
    [Inject] protected AuthSessionState AuthSessionState { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    protected IReadOnlyList<DeviceRowViewModel> Devices { get; private set; } = [];
    protected bool Loading { get; private set; }
    protected string? ErrorMessage { get; private set; }
    protected DeviceInventoryQuery Query { get; private set; } = DeviceInventoryQuery.Default;
    protected int TotalCount { get; private set; }
    protected int TotalPages { get; private set; } = 1;
    protected IReadOnlyList<FilterOptionViewModel> CategoryOptions => DeviceInventoryPresenter.CategoryOptions;
    protected IReadOnlyList<FilterOptionViewModel> StatusOptions => DeviceInventoryPresenter.StatusOptions;

    protected string DisplayName => string.IsNullOrWhiteSpace(AuthSessionState.FullName)
        ? "Administrator"
        : AuthSessionState.FullName;

    protected string AvatarText => string.IsNullOrWhiteSpace(DisplayName)
        ? "A"
        : DisplayName[..1].ToUpperInvariant();

    protected override async Task OnInitializedAsync()
    {
        if (!AuthSessionState.IsAuthenticated)
        {
            NavigationManager.NavigateTo("/");
            return;
        }

        await LoadDevicesAsync();
    }

    protected async Task OnSearchInputAsync(ChangeEventArgs args)
    {
        Query = Query with
        {
            SearchTerm = args.Value?.ToString(),
            PageNumber = 1
        };

        await LoadDevicesAsync();
    }

    protected async Task OnCategoryChangedAsync(ChangeEventArgs args)
    {
        Query = Query with
        {
            CategoryId = args.Value?.ToString() ?? string.Empty,
            PageNumber = 1
        };

        await LoadDevicesAsync();
    }

    protected async Task OnStatusChangedAsync(ChangeEventArgs args)
    {
        Query = Query with
        {
            Status = args.Value?.ToString() ?? string.Empty,
            PageNumber = 1
        };

        await LoadDevicesAsync();
    }

    protected async Task ClearFiltersAsync()
    {
        Query = DeviceInventoryQuery.Default;
        await LoadDevicesAsync();
    }

    protected async Task PreviousPageAsync()
    {
        if (Query.PageNumber <= 1)
        {
            return;
        }

        Query = Query with { PageNumber = Query.PageNumber - 1 };
        await LoadDevicesAsync();
    }

    protected async Task NextPageAsync()
    {
        if (Query.PageNumber >= TotalPages)
        {
            return;
        }

        Query = Query with { PageNumber = Query.PageNumber + 1 };
        await LoadDevicesAsync();
    }

    protected async Task GoToPageAsync(int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > TotalPages || pageNumber == Query.PageNumber)
        {
            return;
        }

        Query = Query with { PageNumber = pageNumber };
        await LoadDevicesAsync();
    }

    protected void OpenCreateDevicePage()
        => NavigationManager.NavigateTo("/devices/new");

    protected string GetDisplayRangeText()
    {
        if (TotalCount == 0)
        {
            return "0 của 0";
        }

        var start = ((Query.PageNumber - 1) * Query.PageSize) + 1;
        var end = Math.Min(Query.PageNumber * Query.PageSize, TotalCount);
        return $"{start}-{end} của {TotalCount:N0}";
    }

    protected IEnumerable<int> GetVisiblePages()
    {
        const int maxPages = 3;

        if (TotalPages <= maxPages)
        {
            return Enumerable.Range(1, TotalPages);
        }

        var start = Math.Max(1, Query.PageNumber - 1);
        if (start + maxPages - 1 > TotalPages)
        {
            start = TotalPages - maxPages + 1;
        }

        return Enumerable.Range(start, maxPages);
    }

    private async Task LoadDevicesAsync(CancellationToken cancellationToken = default)
    {
        Loading = true;
        ErrorMessage = null;

        try
        {
            var request = new GetDeviceInventoryRequestDto(
                Query.SearchTerm,
                Query.CategoryId,
                Query.Status,
                Query.PageNumber,
                Query.PageSize);

            var result = await GetDeviceInventoryUseCase.ExecuteAsync(request, cancellationToken);

            if (result is null)
            {
                ErrorMessage = "Không tải được danh sách thiết bị. Vui lòng đăng nhập lại.";
                return;
            }

            var vm = DeviceInventoryPresenter.ToViewModel(result);
            Devices = vm.Devices;
            TotalCount = vm.TotalCount;
            TotalPages = vm.TotalPages;
        }
        catch
        {
            ErrorMessage = "Có lỗi khi tải danh sách thiết bị.";
        }
        finally
        {
            Loading = false;
        }
    }
}
