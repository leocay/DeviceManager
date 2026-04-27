using DeviceManagerFE.Components;
using DeviceManagerFE.Features.Auth.Application.Interfaces;
using DeviceManagerFE.Features.Auth.Application.UseCases;
using DeviceManagerFE.Features.Auth.Infrastructure.Repositories;
using DeviceManagerFE.Features.Auth.Presentation.Services;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Features.Devices.Application.UseCases;
using DeviceManagerFE.Features.Devices.Infrastructure.Repositories;
using DeviceManagerFE.Features.Devices.Presentation.Services;
using DeviceManagerFE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".DeviceManagerFE.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(8);
});

builder.Services.AddHttpClient<AuthApiClient>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["BackendApi:BaseUrl"] ?? "http://localhost:5144";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<DeviceApiClient>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration["BackendApi:BaseUrl"] ?? "http://localhost:5144";
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<AuthSessionState>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();
builder.Services.AddScoped<ILoginPresenter, LoginPresenter>();
builder.Services.AddScoped<IDeviceInventoryReadRepository, DeviceInventoryReadRepository>();
builder.Services.AddScoped<IDeviceInventoryWriteRepository, DeviceInventoryWriteRepository>();
builder.Services.AddScoped<IEmployeeDirectoryRepository, EmployeeDirectoryRepository>();
builder.Services.AddScoped<IGetDeviceInventoryUseCase, GetDeviceInventoryUseCase>();
builder.Services.AddScoped<ICreateDeviceUseCase, CreateDeviceUseCase>();
builder.Services.AddScoped<IUpdateDeviceUseCase, UpdateDeviceUseCase>();
builder.Services.AddScoped<IGetDeviceDetailUseCase, GetDeviceDetailUseCase>();
builder.Services.AddScoped<IGetEmployeeDirectoryUseCase, GetEmployeeDirectoryUseCase>();
builder.Services.AddScoped<IDeviceInventoryPresenter, DeviceInventoryPresenter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseSession();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

