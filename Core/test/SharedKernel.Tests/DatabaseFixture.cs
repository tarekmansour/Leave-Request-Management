using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Tests;

public abstract class DatabaseFixture : IDisposable
{
    protected readonly ApplicationDbContext _dbContext;

    protected DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LeaveManagement_{Guid.NewGuid():N};Integrated Security=True;Connect Timeout=5;TrustServerCertificate=true;")
            .EnableSensitiveDataLogging()
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.Migrate();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
