using System.Net.Http.Json;
using DeviceManagerBE.Contracts.Auth;

namespace DeviceManagerFE.Services;

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
                Message = $"Không kết nối được backend tại {_httpClient.BaseAddress}. Hãy đảm bảo API đang chạy."
            };
        }
        catch (TaskCanceledException)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Yêu cầu đến backend bị timeout. Hãy thử lại sau."
            };
        }

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken);
            return result ?? new LoginResponse { Success = false, Message = "Phản hồi từ API không hợp lệ." };
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new LoginResponse { Success = false, Message = "Sai tên đăng nhập hoặc mật khẩu." };
        }

        return new LoginResponse
        {
            Success = false,
            Message = $"Đăng nhập thất bại. HTTP {(int)response.StatusCode}."
        };
    }
}

