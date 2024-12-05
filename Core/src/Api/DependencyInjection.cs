using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services) =>
        services
            .AddEndpointsApiExplorer()
            .AddControllersOptions()
            .AddSwaggerGenInternal()
            .AddApiVersioningManagement()
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

    private static IServiceCollection AddControllersOptions(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Add support for serializing enums as strings
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

                // Ignore null values in JSON responses
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

        return services;
    }

    private static IServiceCollection AddApiVersioningManagement(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    private static IServiceCollection AddSwaggerGenInternal(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "LeaveManagement Api",
                Version = "v1",
                Description = "The Leave Management API is designed to facilitate the management of leave requests within an organization." +
                "This API provides a comprehensive set of endpoints that allow employees, managers, and HR personnel to efficiently handle various aspects of leave management," +
                "including requesting, approving, and tracking leave requests."
            });
            c.SupportNonNullableReferenceTypes();
            c.EnableAnnotations();
        });

        return services;
    }
}
