using DeviceManager.Contracts.Auth;

namespace DeviceManager.WebApp.Services;

public class AuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
            return result ?? new LoginResponse { Success = false, Message = "Phan hoi tu API khong hop le." };
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new LoginResponse { Success = false, Message = "Sai ten dang nhap hoac mat khau." };
        }

        return new LoginResponse
        {
            Success = false,
            Message = $"Dang nhap that bai. HTTP {(int)response.StatusCode}."
        };
    }
}