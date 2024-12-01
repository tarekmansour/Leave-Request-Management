using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

    public DbSet<LeaveRequest> LeaveRequests { get; set; } = default!;
    public DbSet<LeaveType> LeaveTypes { get; set; } = default!;
    public DbSet<Employee> Employees { get; set; } = default!;
}
