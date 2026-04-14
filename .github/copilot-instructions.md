# Project Guidelines

## Code Style
- Target .NET 8 with nullable reference types and implicit usings enabled.
- Keep API entry points minimal; use extension methods for composition, as shown in [src/DeviceManagerBE/Program.cs](src/DeviceManagerBE/Program.cs) and [src/DeviceManagerBE.Infrastructure/DependencyInjection.cs](src/DeviceManagerBE.Infrastructure/DependencyInjection.cs).
- Prefer EF Core fluent configuration classes over data annotations. Existing examples live in [src/DeviceManagerBE.Infrastructure/Persistence/Configurations/DeviceConfiguration.cs](src/DeviceManagerBE.Infrastructure/Persistence/Configurations/DeviceConfiguration.cs).

## Architecture
- Follow the current clean-architecture split: Domain for entities, Application for business logic, Contracts for shared DTOs/contracts, Infrastructure for persistence and external services, and Api as the host.
- Register persistence through [src/DeviceManagerBE.Infrastructure/Persistence/ApplicationDbContext.cs](src/DeviceManagerBE.Infrastructure/Persistence/ApplicationDbContext.cs) and let it discover `IEntityTypeConfiguration<T>` implementations from the assembly.
- Keep domain entities free of infrastructure concerns; put SQL Server and EF Core specifics in Infrastructure.

## Build And Test
- Start the database with `docker compose -f database/docker-compose.yml up -d`.
- Run the API with `dotnet run --project src/DeviceManagerBE/DeviceManagerBE.csproj`.
- Build the solution with `dotnet build src/src.sln`.
- Create or apply EF Core migrations using the commands documented in [database/README.md](database/README.md).
- There is no test project in the repository yet; add one before introducing new automated test coverage.

## Conventions
- Use `AddInfrastructure` to register EF Core and keep connection-string handling in configuration.
- Keep database defaults aligned with [src/DeviceManagerBE/appsettings.Development.json](src/DeviceManagerBE/appsettings.Development.json) and the SQL Server container in [database/docker-compose.yml](database/docker-compose.yml).
- Preserve the current port/profile setup from [src/DeviceManagerBE/Properties/launchSettings.json](src/DeviceManagerBE/Properties/launchSettings.json) unless there is a deliberate reason to change it.
- For database work, link to [database/README.md](database/README.md) instead of duplicating setup steps here.