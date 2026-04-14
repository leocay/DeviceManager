using DeviceManagerBE.Application;
using DeviceManagerBE.Middleware;
using DeviceManagerBE.Infrastructure;
using DeviceManagerBE.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtOptions = builder.Configuration.GetSection(JwtTokenOptions.SectionName).Get<JwtTokenOptions>()
	?? throw new InvalidOperationException("Jwt configuration is missing.");

if (string.IsNullOrWhiteSpace(jwtOptions.SigningKey) || jwtOptions.SigningKey.Length < 32)
{
	throw new InvalidOperationException("Jwt:SigningKey must be configured and at least 32 characters long.");
}

builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.InvalidModelStateResponseFactory = context =>
	{
		var problemDetails = new ValidationProblemDetails(context.ModelState)
		{
			Type = "https://httpstatuses.com/400",
			Title = "Validation Failed",
			Detail = "One or more validation errors occurred.",
			Status = StatusCodes.Status400BadRequest,
			Instance = context.HttpContext.Request.Path
		};

		problemDetails.Extensions["errorCode"] = "VALIDATION_ERROR";
		problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
		return new BadRequestObjectResult(problemDetails);
	};
});

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
});

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtOptions.Issuer,
			ValidAudience = jwtOptions.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
			ClockSkew = TimeSpan.FromMinutes(1)
		};
	});

builder.Services.AddAuthorization();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


