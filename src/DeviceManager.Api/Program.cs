using DeviceManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    Project = "DeviceManager",
    Status = "Running"
}));

app.Run();
