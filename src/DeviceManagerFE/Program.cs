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
using Microsoft.AspNetCore.HttpOverrides;

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
builder.Services.AddScoped<BrowserAuthSessionRestorer>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();
builder.Services.AddScoped<ILoginPresenter, LoginPresenter>();
builder.Services.AddScoped<IDeviceInventoryReadRepository, DeviceInventoryReadRepository>();
builder.Services.AddScoped<IDeviceInventoryWriteRepository, DeviceInventoryWriteRepository>();
builder.Services.AddScoped<IEmployeeDirectoryRepository, EmployeeDirectoryRepository>();
builder.Services.AddScoped<IGetDeviceInventoryUseCase, GetDeviceInventoryUseCase>();
builder.Services.AddScoped<IDeleteDeviceUseCase, DeleteDeviceUseCase>();
builder.Services.AddScoped<ICreateDeviceUseCase, CreateDeviceUseCase>();
builder.Services.AddScoped<IUpdateDeviceUseCase, UpdateDeviceUseCase>();
builder.Services.AddScoped<IGetDeviceDetailUseCase, GetDeviceDetailUseCase>();
builder.Services.AddScoped<IGetEmployeeDirectoryUseCase, GetEmployeeDirectoryUseCase>();
builder.Services.AddScoped<IDeviceInventoryPresenter, DeviceInventoryPresenter>();

var app = builder.Build();

var useForwardedHeaders = builder.Configuration.GetValue("HttpPipeline:UseForwardedHeaders", true);
var forceHttps = builder.Configuration.GetValue("HttpPipeline:ForceHttps", false);

if (useForwardedHeaders)
{
    var forwardedHeadersOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        ForwardLimit = 1
    };

    forwardedHeadersOptions.KnownNetworks.Clear();
    forwardedHeadersOptions.KnownProxies.Clear();

    app.UseForwardedHeaders(forwardedHeadersOptions);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    if (forceHttps)
    {
        // Only enable HSTS when the origin is allowed to serve HTTPS directly.
        app.UseHsts();
    }
}

if (forceHttps)
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseSession();
app.UseAntiforgery();

// Populate AuthSessionState from server session on each request so components
// see persisted authentication after a full-page reload.
app.Use(async (context, next) =>
{
    try
    {
        var auth = context.RequestServices.GetService(typeof(DeviceManagerFE.Services.AuthSessionState)) as DeviceManagerFE.Services.AuthSessionState;
        if (auth is not null)
        {
            var token = context.Session.GetString("__Auth_AccessToken");
            var name = context.Session.GetString("__Auth_FullName");

            // fallback: read from request cookies if session not populated
            if (string.IsNullOrWhiteSpace(token) && context.Request.Cookies.ContainsKey("__Auth_AccessToken"))
            {
                token = context.Request.Cookies["__Auth_AccessToken"];
            }

            if (string.IsNullOrWhiteSpace(name) && context.Request.Cookies.ContainsKey("__Auth_FullName"))
            {
                name = context.Request.Cookies["__Auth_FullName"];
            }

            auth.AccessToken = string.IsNullOrWhiteSpace(token) ? null : token;
            auth.FullName = string.IsNullOrWhiteSpace(name) ? null : name;
        }
    }
    catch
    {
        // best effort; don't fail request if session not available
    }

    await next();
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

