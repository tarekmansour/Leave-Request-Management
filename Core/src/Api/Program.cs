using Api;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersOptions()
    .AddEndpointsApiExplorer()
    .AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddApiVersioningManagement()
    .AddEndpointsApiExplorer()
    .AddSwagger()
    .AddApplication()
    .AddInfrastructure();

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
