# DeviceManager Database

Project này đang dùng EF Core Code First với SQL Server Docker.

## SQL Server container

```powershell
docker compose -f database/docker-compose.yml up -d
```

Container hiện dùng:

- `sqlserver-container`
- `localhost:1433`
- `sa / YourStrong@Passw0rd`

## Connection string

```text
Server=localhost,1433;Database=DeviceManagerDb;User Id=sa;Password=YourStrong@Passw0rd;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True
```

## Tạo migration

```powershell
dotnet ef migrations add InitialCreate --no-build --project src/DeviceManager.Infrastructure/DeviceManager.Infrastructure.csproj --startup-project src/DeviceManager.Api/DeviceManager.Api.csproj --context DeviceManager.Infrastructure.Persistence.ApplicationDbContext --output-dir Persistence/Migrations
```

## Apply migration vào database

```powershell
dotnet ef database update --no-build --project src/DeviceManager.Infrastructure/DeviceManager.Infrastructure.csproj --startup-project src/DeviceManager.Api/DeviceManager.Api.csproj --context DeviceManager.Infrastructure.Persistence.ApplicationDbContext
```

## Tài khoản admin mặc định

Khi tạo database lần đầu, hệ thống sẽ seed sẵn một tài khoản admin:

- Username: `admin`
- Password: `Admin@123456`

Mật khẩu được lưu dưới dạng hash trong database. Nên đổi ngay sau khi đăng nhập lần đầu.
