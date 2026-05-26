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
dotnet ef migrations add InitialCreate --no-build --project src/DeviceManagerBE.Infrastructure/DeviceManagerBE.Infrastructure.csproj --startup-project src/DeviceManagerBE/DeviceManagerBE.csproj --context DeviceManagerBE.Infrastructure.Persistence.ApplicationDbContext --output-dir Persistence/Migrations
```

## Apply migration vào database

```powershell
dotnet ef database update --no-build --project src/DeviceManagerBE.Infrastructure/DeviceManagerBE.Infrastructure.csproj --startup-project src/DeviceManagerBE/DeviceManagerBE.csproj --context DeviceManagerBE.Infrastructure.Persistence.ApplicationDbContext
```

## Tài khoản admin mặc định

Khi tạo database lần đầu, hệ thống sẽ seed sẵn một tài khoản admin:

- Username: `admin`
- Password: `Admin@123456`

Mật khẩu được lưu dưới dạng hash trong database. Nên đổi ngay sau khi đăng nhập lần đầu.

## publish
```powershell
dotnet publish src\DeviceManagerBE\DeviceManagerBE.csproj -c Release -o publish\be
dotnet publish src\DeviceManagerFE\DeviceManagerFE.csproj -c Release -o publish\fe
```

## migration 
```powerShell
cd D:\LINH\DEV\DeviceManager
$env:ConnectionStrings__DefaultConnection="Server=103.170.123.126,1433;Database=DeviceManagerDb;User Id=devicemanager_user;Password=Linh@3181992;Encrypt=False;TrustServerCertificate=True;MultipleActiveResultSets=True"
$env:Jwt__Issuer="DeviceManagerBE"
$env:Jwt__Audience="DeviceManagerFE"
$env:Jwt__SigningKey="HayDoiChuoiNayThanhSecretRatDaiToiThieu32KyTu_Production_2026"
$env:Jwt__AccessTokenMinutes="60"

dotnet ef database update --project src\DeviceManagerBE.Infrastructure\DeviceManagerBE.Infrastructure.csproj --startup-project src\DeviceManagerBE\DeviceManagerBE.csproj --context DeviceManagerBE.Infrastructure.Persistence.ApplicationDbContext
```

