using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Tests;

public abstract class DatabaseFixture : IDisposable
{
    protected readonly ApplicationDbContext _dbContext;

    protected DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"LeaveManagement_{Guid.NewGuid()}")
            .EnableSensitiveDataLogging()
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureDeleted();
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
