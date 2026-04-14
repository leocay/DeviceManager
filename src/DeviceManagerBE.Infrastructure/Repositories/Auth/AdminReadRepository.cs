using DeviceManagerBE.Application.Services.Auth;
using DeviceManagerBE.Domain.Entities;
using DeviceManagerBE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagerBE.Infrastructure.Repositories.Auth;

public class AdminReadRepository : IAdminReadRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AdminReadRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Admin?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _dbContext.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }
}
