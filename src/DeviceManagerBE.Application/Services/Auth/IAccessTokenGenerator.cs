namespace DeviceManagerBE.Application.Services.Auth;

public interface IAccessTokenGenerator
{
    AccessTokenResult Generate(int adminId, string username, string fullName);
}
