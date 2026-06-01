using DeviceManagerBE.Application.DTOs.Device;
using DeviceManagerBE.Application.Services.Device;
using DeviceManagerBE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagerBE.Infrastructure.Repositories.Device;

public class DeviceReadRepository : IDeviceReadRepository
{
    private const string AccentInsensitiveSearchCollation = "Latin1_General_100_CI_AI";
    private readonly ApplicationDbContext _dbContext;

    public DeviceReadRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetDevicesResultDto> GetDevicesPaginatedAsync(
        string? searchTerm,
        string? categoryId,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Devices
            .Include(d => d.Category)
            .Include(d => d.Employee)
            .AsNoTracking()
            .Where(d => d.Status != "Deleted")
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchPattern = $"%{EscapeLikePattern(searchTerm.Trim())}%";

            query = query.Where(d =>
                EF.Functions.Like(
                    EF.Functions.Collate(d.DeviceCode, AccentInsensitiveSearchCollation),
                    searchPattern,
                    @"\") ||
                (d.Employee != null &&
                    EF.Functions.Like(
                        EF.Functions.Collate(d.Employee.FullName, AccentInsensitiveSearchCollation),
                        searchPattern,
                        @"\")));
        }

        // Apply category filter
        if (!string.IsNullOrWhiteSpace(categoryId) && int.TryParse(categoryId, out var parsedCategoryId))
        {
            query = query.Where(d => d.CategoryId == parsedCategoryId);
        }

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(d => d.Status == status.Trim());
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Calculate pagination
        var totalPages = (totalCount + pageSize - 1) / pageSize;
        var hasNextPage = pageNumber < totalPages;
        var hasPreviousPage = pageNumber > 1;

        // Apply pagination
        var devices = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DeviceItemDto
            {
                DeviceId = d.DeviceId,
                DeviceCode = d.DeviceCode,
                DeviceName = d.DeviceName,
                CategoryName = d.Category != null ? d.Category.CategoryName : string.Empty,
                EmployeeName = d.Employee != null ? d.Employee.FullName : null,
                SerialNumber = d.SerialNumber,
                Status = d.Status,
                Brand = d.Brand,
                Model = d.Model
            })
            .ToListAsync(cancellationToken);

        return new GetDevicesResultDto
        {
            Devices = devices,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasPreviousPage = hasPreviousPage,
            HasNextPage = hasNextPage
        };
    }

    public async Task<DeviceDetailDto?> GetByIdAsync(
        int deviceId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Devices
            .Include(d => d.Category)
            .Include(d => d.DeviceLogs)
            .AsNoTracking()
            .Where(d => d.DeviceId == deviceId && d.Status != "Deleted")
            .Select(d => new DeviceDetailDto
            {
                DeviceId = d.DeviceId,
                DeviceCode = d.DeviceCode,
                DeviceName = d.DeviceName,
                CategoryId = d.CategoryId,
                CategoryName = d.Category != null ? d.Category.CategoryName : string.Empty,
                CategoryDescription = d.Category != null ? d.Category.Description : null,
                EmployeeId = d.EmployeeId,
                Brand = d.Brand,
                Model = d.Model,
                SerialNumber = d.SerialNumber,
                PurchaseDate = d.PurchaseDate,
                WarrantyExpiryDate = d.WarrantyExpiryDate,
                Status = d.Status,
                Note = d.Note,
                Logs = d.DeviceLogs
                    .OrderByDescending(log => log.ActionTime)
                    .Select(log => new DeviceLogItemDto
                    {
                        ActionType = log.ActionType,
                        ActionBy = log.ActionBy,
                        ActionTime = log.ActionTime,
                        Content = log.Content
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static string EscapeLikePattern(string value)
    {
        return value
            .Replace(@"\", @"\\", StringComparison.Ordinal)
            .Replace("%", @"\%", StringComparison.Ordinal)
            .Replace("_", @"\_", StringComparison.Ordinal)
            .Replace("[", "[[]", StringComparison.Ordinal);
    }
}

