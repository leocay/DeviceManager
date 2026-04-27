using System.Net.Http.Headers;
using System.Net.Http.Json;
using DeviceManagerBE.Contracts.Device;
using DeviceManagerBE.Contracts.Employee;
using DeviceManagerFE.Features.Devices.Application.DTOs;

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

    public async Task<CreateDeviceResultDto> CreateDeviceAsync(
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!_authSessionState.IsAuthenticated)
        {
            return new CreateDeviceResultDto
            {
                Success = false,
                Message = "Phien dang nhap da het han."
            };
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
            return new CreateDeviceResultDto
            {
                Success = false,
                Message = $"Khong ket noi duoc backend tai {_httpClient.BaseAddress}."
            };
        }
        catch (TaskCanceledException)
        {
            return new CreateDeviceResultDto
            {
                Success = false,
                Message = "Yeu cau tao thiet bi bi timeout."
            };
        }

        if (response.IsSuccessStatusCode)
        {
            var created = await response.Content.ReadFromJsonAsync<CreateDeviceResponse>(cancellationToken: cancellationToken);

            return new CreateDeviceResultDto
            {
                Success = true,
                Message = created?.Message ?? "Tao moi thiet bi thanh cong.",
                DeviceId = created?.DeviceId
            };
        }

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var problem = await response.Content.ReadFromJsonAsync<DeviceValidationProblemResponse>(cancellationToken: cancellationToken);

            return new CreateDeviceResultDto
            {
                Success = false,
                Message = problem?.Detail ?? "Du lieu tao thiet bi khong hop le.",
                ValidationErrors = problem?.Errors?.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value,
                    StringComparer.Ordinal) ?? new Dictionary<string, string[]>(StringComparer.Ordinal)
            };
        }

        return new CreateDeviceResultDto
        {
            Success = false,
            Message = $"Tao moi thiet bi that bai. HTTP {(int)response.StatusCode}."
        };
    }

    public async Task<IReadOnlyList<Features.Devices.Application.DTOs.EmployeeOptionDto>> GetEmployeesAsync(CancellationToken cancellationToken = default)
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
            .Select(employee => new Features.Devices.Application.DTOs.EmployeeOptionDto
            {
                EmployeeId = employee.EmployeeId,
                DisplayName = employee.DisplayName
            })
            .ToList() ?? [];
    }

    private sealed class DeviceValidationProblemResponse
    {
        public string? Detail { get; init; }
        public Dictionary<string, string[]> Errors { get; init; } = new(StringComparer.Ordinal);
    }
}
