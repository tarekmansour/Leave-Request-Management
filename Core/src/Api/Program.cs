using Api;
using Application;
using Asp.Versioning;
using Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddApiVersioning(options =>
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

builder.Services
    .AddApplication()
    .AddInfrastructure();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStatusCodePages();

app.UseExceptionHandler();

app.MapControllers();

app.Run();

namespace Api
{
    public partial class Program;
}
