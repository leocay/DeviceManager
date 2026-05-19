using DeviceManagerBE.Application.DTOs.Device;
using DeviceManagerBE.Application.Services.Device;
using DeviceManagerBE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagerBE.Infrastructure.Repositories.Device;

public class DeviceReadRepository : IDeviceReadRepository
{
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
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(d =>
                d.DeviceCode.Contains(searchTerm) ||
                (d.Employee != null && d.Employee.FullName.Contains(searchTerm)));
        }

        // Apply category filter
        if (!string.IsNullOrWhiteSpace(categoryId) && int.TryParse(categoryId, out var parsedCategoryId))
        {
            query = query.Where(d => d.CategoryId == parsedCategoryId);
        }

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(d => d.Status == status);
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
            .AsNoTracking()
            .Where(d => d.DeviceId == deviceId)
            .Select(d => new DeviceDetailDto
            {
                DeviceId = d.DeviceId,
                DeviceCode = d.DeviceCode,
                DeviceName = d.DeviceName,
                CategoryId = d.CategoryId,
                CategoryName = d.Category != null ? d.Category.CategoryName : string.Empty,
                EmployeeId = d.EmployeeId,
                Brand = d.Brand,
                Model = d.Model,
                SerialNumber = d.SerialNumber,
                PurchaseDate = d.PurchaseDate,
                WarrantyExpiryDate = d.WarrantyExpiryDate,
                Status = d.Status,
                Note = d.Note
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

