using DeviceManagerBE.Application.DTOs.Employee;
using DeviceManagerBE.Application.Services.Employee;
using DeviceManagerBE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagerBE.Infrastructure.Repositories.Employee;

public sealed class EmployeeReadRepository : IEmployeeReadRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeReadRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<EmployeeOptionResultDto>> GetEmployeesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Employees
            .AsNoTracking()
            .OrderBy(employee => employee.FullName)
            .Select(employee => new EmployeeOptionResultDto
            {
                EmployeeId = employee.EmployeeId,
                EmployeeCode = employee.EmployeeCode,
                FullName = employee.FullName,
                Department = employee.Department
            })
            .ToListAsync(cancellationToken);
    }
}
