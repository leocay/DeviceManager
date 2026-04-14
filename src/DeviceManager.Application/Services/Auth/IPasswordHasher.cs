namespace DeviceManager.Application.Services.Auth;

public interface IPasswordHasher
{
    string Hash(string rawPassword);
}