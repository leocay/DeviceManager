# Project Guidelines

## Code Style
- Target .NET 8 with nullable reference types and implicit usings enabled.
- Keep API entry points minimal; use extension methods for composition, as shown in [src/DeviceManager.Api/Program.cs](src/DeviceManager.Api/Program.cs) and [src/DeviceManager.Infrastructure/DependencyInjection.cs](src/DeviceManager.Infrastructure/DependencyInjection.cs).
- Prefer EF Core fluent configuration classes over data annotations. Existing examples live in [src/DeviceManager.Infrastructure/Persistence/Configurations/DeviceConfiguration.cs](src/DeviceManager.Infrastructure/Persistence/Configurations/DeviceConfiguration.cs).

## Architecture
- Follow the current clean-architecture split: Domain for entities, Application for business logic, Contracts for shared DTOs/contracts, Infrastructure for persistence and external services, and Api as the host.
- Register persistence through [src/DeviceManager.Infrastructure/Persistence/ApplicationDbContext.cs](src/DeviceManager.Infrastructure/Persistence/ApplicationDbContext.cs) and let it discover `IEntityTypeConfiguration<T>` implementations from the assembly.
- Keep domain entities free of infrastructure concerns; put SQL Server and EF Core specifics in Infrastructure.

## Build And Test
- Start the database with `docker compose -f database/docker-compose.yml up -d`.
- Run the API with `dotnet run --project src/DeviceManager.Api/DeviceManager.Api.csproj`.
- Build the solution with `dotnet build src/src.sln`.
- Create or apply EF Core migrations using the commands documented in [database/README.md](database/README.md).
- There is no test project in the repository yet; add one before introducing new automated test coverage.

## Conventions
- Use `AddInfrastructure` to register EF Core and keep connection-string handling in configuration.
- Keep database defaults aligned with [src/DeviceManager.Api/appsettings.Development.json](src/DeviceManager.Api/appsettings.Development.json) and the SQL Server container in [database/docker-compose.yml](database/docker-compose.yml).
- Preserve the current port/profile setup from [src/DeviceManager.Api/Properties/launchSettings.json](src/DeviceManager.Api/Properties/launchSettings.json) unless there is a deliberate reason to change it.
- For database work, link to [database/README.md](database/README.md) instead of duplicating setup steps here.