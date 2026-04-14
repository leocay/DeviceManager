using DeviceManagerBE.Application.Services.Auth;
using DeviceManagerBE.Application.Services.Device;
using DeviceManagerBE.Infrastructure.Auth;
using DeviceManagerBE.Infrastructure.Persistence;
using DeviceManagerBE.Infrastructure.Security;
using DeviceManagerBE.Infrastructure.Repositories.Auth;
using DeviceManagerBE.Infrastructure.Repositories.Device;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagerBE.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.Configure<JwtTokenOptions>(options =>
        {
            configuration.GetSection(JwtTokenOptions.SectionName).Bind(options);
        });

        services.AddScoped<IAdminReadRepository, AdminReadRepository>();
        services.AddScoped<IDeviceReadRepository, DeviceReadRepository>();
        services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();
        services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();

        return services;
    }
}

