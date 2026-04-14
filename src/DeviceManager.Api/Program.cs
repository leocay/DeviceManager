using DeviceManager.Infrastructure;
using DeviceManager.Infrastructure.Persistence;
using DeviceManager.Contracts.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Project = "DeviceManager",
    Status = "Running"
}));

app.MapPost("/api/auth/login", async (LoginRequest request, ApplicationDbContext dbContext) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new LoginResponse
        {
            Success = false,
            Message = "Vui long nhap day du ten dang nhap va mat khau."
        });
    }

    var passwordHash = HashPassword(request.Password);

    var admin = await dbContext.Admins
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Username == request.Username && x.PasswordHash == passwordHash);

    if (admin is null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new LoginResponse
    {
        Success = true,
        Message = "Dang nhap thanh cong.",
        FullName = admin.FullName
    });
});

app.Run();

static string HashPassword(string password)
{
    var bytes = Encoding.UTF8.GetBytes(password);
    var hash = SHA256.HashData(bytes);
    return Convert.ToHexString(hash).ToLowerInvariant();
}
