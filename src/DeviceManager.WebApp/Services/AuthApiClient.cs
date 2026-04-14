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
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("/api/auth/login", request, cancellationToken);
        }
        catch (HttpRequestException)
        {
            return new LoginResponse
            {
                Success = false,
                Message = $"Khong ket noi duoc backend tai {_httpClient.BaseAddress}. Hay dam bao API dang chay."
            };
        }
        catch (TaskCanceledException)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Yeu cau den backend bi timeout. Hay thu lai sau."
            };
        }

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