using DeviceManagerBE.Application.Services.Auth;
using System.Security.Cryptography;
using System.Text;

namespace DeviceManagerBE.Infrastructure.Auth;

public class Sha256PasswordHasher : IPasswordHasher
{
    public string Hash(string rawPassword)
    {
        var bytes = Encoding.UTF8.GetBytes(rawPassword);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
