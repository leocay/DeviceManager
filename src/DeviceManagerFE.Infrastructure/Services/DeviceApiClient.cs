using System.Net.Http.Headers;
using System.Net.Http.Json;
using DeviceManagerBE.Contracts.Device;

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
}


