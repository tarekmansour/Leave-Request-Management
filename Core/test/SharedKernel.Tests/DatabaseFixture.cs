using System.Diagnostics.CodeAnalysis;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Tests;

[ExcludeFromCodeCoverage]
public abstract class DatabaseFixture : IDisposable
{
    protected readonly ApplicationDbContext _dbContext;
    private bool _disposed = false;

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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources
                _dbContext.Database.EnsureDeleted();
                _dbContext.Dispose();
            }

            // Dispose unmanaged resources here if there are any
            _disposed = true;
        }
    }
}
