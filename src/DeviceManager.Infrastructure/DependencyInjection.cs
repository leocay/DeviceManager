using DeviceManager.Application.Services.Auth;
using DeviceManager.Infrastructure.Auth;
using DeviceManager.Infrastructure.Persistence;
using DeviceManager.Infrastructure.Security;
using DeviceManager.Infrastructure.Repositories.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManager.Infrastructure;

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
        services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();
        services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();

        return services;
    }
}
