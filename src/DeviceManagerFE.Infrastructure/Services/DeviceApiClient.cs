using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DeviceManagerBE.Contracts.Device;
using DeviceManagerBE.Contracts.Employee;
using DeviceManagerFE.Features.Devices.Application.DTOs;
using EmployeeOptionViewDto = DeviceManagerFE.Features.Devices.Application.DTOs.EmployeeOptionDto;

namespace DeviceManagerFE.Services;

public class DeviceApiClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthSessionState _authSessionState;

    public DeviceApiClient(HttpClient httpClient, AuthSessionState authSessionState)
    {
        _httpClient = httpClient;
        _authSessionState = authSessionState;
    }

    public async Task<GetDevicesResponse?> GetDevicesAsync(
        string? searchTerm,
        string? categoryId,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (!_authSessionState.IsAuthenticated)
        {
            return null;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authSessionState.AccessToken);

        var queryParts = new List<string>
        {
            $"pageNumber={pageNumber}",
            $"pageSize={pageSize}"
        };

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryParts.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
        }

        if (!string.IsNullOrWhiteSpace(categoryId))
        {
            queryParts.Add($"categoryId={Uri.EscapeDataString(categoryId)}");
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            queryParts.Add($"status={Uri.EscapeDataString(status)}");
        }

        var url = $"/api/v1/devices?{string.Join("&", queryParts)}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<GetDevicesResponse>(cancellationToken: cancellationToken);
    }

    public async Task<DeviceEditorDto?> GetDeviceByIdAsync(
        int deviceId,
        CancellationToken cancellationToken = default)
    {
        if (!_authSessionState.IsAuthenticated)
        {
            return null;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authSessionState.AccessToken);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync($"/api/v1/devices/{deviceId}", cancellationToken);
        }
        catch
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<GetDeviceDetailResponse>(cancellationToken: cancellationToken);
        if (result is null)
        {
            return null;
        }

        return new DeviceEditorDto
        {
            DeviceId = result.DeviceId,
            DeviceCode = result.DeviceCode,
            DeviceName = result.DeviceName,
            CategoryId = result.CategoryId,
            EmployeeId = result.EmployeeId,
            Brand = result.Brand,
            Model = result.Model,
            SerialNumber = result.SerialNumber,
            PurchaseDate = result.PurchaseDate,
            WarrantyExpiryDate = result.WarrantyExpiryDate,
            Status = result.Status,
            Note = result.Note
        };
    }

    public async Task<CreateDeviceResultDto> CreateDeviceAsync(
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!_authSessionState.IsAuthenticated)
        {
            return BuildSessionExpiredResult();
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authSessionState.AccessToken);

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync("/api/v1/devices", new CreateDeviceRequest
            {
                DeviceCode = request.DeviceCode,
                DeviceName = request.DeviceName,
                CategoryId = request.CategoryId,
                Brand = request.Brand,
                Model = request.Model,
                SerialNumber = request.SerialNumber,
                PurchaseDate = request.PurchaseDate,
                WarrantyExpiryDate = request.WarrantyExpiryDate,
                Status = request.Status,
                EmployeeId = request.EmployeeId,
                Note = request.Note
            }, cancellationToken);
        }
        catch (HttpRequestException)
        {
            return BuildConnectionErrorResult();
        }
        catch (TaskCanceledException)
        {
            return new CreateDeviceResultDto
            {
                Success = false,
                Message = "Yêu cầu tạo thiết bị bị timeout."
            };
        }

        if (response.IsSuccessStatusCode)
        {
            var created = await response.Content.ReadFromJsonAsync<CreateDeviceResponse>(cancellationToken: cancellationToken);

            return new CreateDeviceResultDto
            {
                Success = true,
                Message = created?.Message ?? "Tạo mới thiết bị thành công.",
                DeviceId = created?.DeviceId
            };
        }

        return await BuildFailureResultAsync(
            response,
            "Dữ liệu tạo thiết bị không hợp lệ.",
            "Tạo mới thiết bị thất bại.",
            cancellationToken);
    }

    public async Task<CreateDeviceResultDto> UpdateDeviceAsync(
        int deviceId,
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!_authSessionState.IsAuthenticated)
        {
            return BuildSessionExpiredResult();
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authSessionState.AccessToken);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PutAsJsonAsync($"/api/v1/devices/{deviceId}", new UpdateDeviceRequest
            {
                DeviceCode = request.DeviceCode,
                DeviceName = request.DeviceName,
                CategoryId = request.CategoryId,
                Brand = request.Brand,
                Model = request.Model,
                SerialNumber = request.SerialNumber,
                PurchaseDate = request.PurchaseDate,
                WarrantyExpiryDate = request.WarrantyExpiryDate,
                Status = request.Status,
                EmployeeId = request.EmployeeId,
                Note = request.Note
            }, cancellationToken);
        }
        catch (HttpRequestException)
        {
            return BuildConnectionErrorResult();
        }
        catch (TaskCanceledException)
        {
            return new CreateDeviceResultDto
            {
                Success = false,
                Message = "Yêu cầu cập nhật thiết bị bị timeout."
            };
        }

        if (response.IsSuccessStatusCode)
        {
            var updated = await response.Content.ReadFromJsonAsync<UpdateDeviceResponse>(cancellationToken: cancellationToken);
            return new CreateDeviceResultDto
            {
                Success = true,
                Message = updated?.Message ?? "Cập nhật thiết bị thành công.",
                DeviceId = updated?.DeviceId
            };
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new CreateDeviceResultDto
            {
                Success = false,
                Message = "Thiết bị không tồn tại hoặc đã bị xóa."
            };
        }

        return await BuildFailureResultAsync(
            response,
            "Dữ liệu cập nhật thiết bị không hợp lệ.",
            "Cập nhật thiết bị thất bại.",
            cancellationToken);
    }

    public async Task<IReadOnlyList<EmployeeOptionViewDto>> GetEmployeesAsync(CancellationToken cancellationToken = default)
    {
        if (!_authSessionState.IsAuthenticated)
        {
            return [];
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authSessionState.AccessToken);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("/api/v1/employees", cancellationToken);
        }
        catch
        {
            return [];
        }

        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        var result = await response.Content.ReadFromJsonAsync<GetEmployeesResponse>(cancellationToken: cancellationToken);
        return result?.Employees
            .Select(employee => new EmployeeOptionViewDto
            {
                EmployeeId = employee.EmployeeId,
                DisplayName = employee.DisplayName
            })
            .ToList() ?? [];
    }

    private CreateDeviceResultDto BuildSessionExpiredResult() => new()
    {
        Success = false,
        Message = "Phiên đăng nhập đã hết hạn."
    };

    private CreateDeviceResultDto BuildConnectionErrorResult() => new()
    {
        Success = false,
        Message = $"Không kết nối được backend tại {_httpClient.BaseAddress}."
    };

    private async Task<CreateDeviceResultDto> BuildFailureResultAsync(
        HttpResponseMessage response,
        string badRequestFallback,
        string genericFailurePrefix,
        CancellationToken cancellationToken)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<DeviceValidationProblemResponse>(cancellationToken: cancellationToken);

            return new CreateDeviceResultDto
            {
                Success = false,
                Message = problem?.Detail ?? badRequestFallback,
                ValidationErrors = problem?.Errors?.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value,
                    StringComparer.Ordinal) ?? new Dictionary<string, string[]>(StringComparer.Ordinal)
            };
        }

        return new CreateDeviceResultDto
        {
            Success = false,
            Message = $"{genericFailurePrefix} HTTP {(int)response.StatusCode}."
        };
    }

    private sealed class DeviceValidationProblemResponse
    {
        public string? Detail { get; init; }
        public Dictionary<string, string[]> Errors { get; init; } = new(StringComparer.Ordinal);
    }
}
