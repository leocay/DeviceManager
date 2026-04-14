using DeviceManager.Application.Services.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeviceManager.Infrastructure.Security;

public class JwtAccessTokenGenerator : IAccessTokenGenerator
{
    private readonly JwtTokenOptions _options;

    public JwtAccessTokenGenerator(IOptions<JwtTokenOptions> options)
    {
        _options = options.Value;
    }

    public AccessTokenResult Generate(int adminId, string username, string fullName)
    {
        var now = DateTime.UtcNow;
        var expiresAtUtc = now.AddMinutes(_options.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, adminId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, username),
            new(JwtRegisteredClaimNames.Name, fullName),
            new(ClaimTypes.NameIdentifier, adminId.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return new AccessTokenResult(token, expiresAtUtc);
    }
}