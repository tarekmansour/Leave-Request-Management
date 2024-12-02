using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("LeaveManagementDb"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

        return services;
    }
}
