using Microsoft.EntityFrameworkCore;
using PublishingHouse.Api.Core;
using PublishingHouse.Application;
using PublishingHouse.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRouting()
    .AddEndpointsApiExplorer()
    .AddProblemDetails()
    .AddSwaggerGen()
    .AddPublishingHouse(builder.Configuration)
    .AddControllers();

var app = builder.Build();

app.UseRouting()
    .UseRouting()
    .UseAuthorization()
    .UseExceptionHandlerWithDefaultMapping()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    })
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Publishing House V1");
        c.RoutePrefix = string.Empty;
    });

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var useInMemory = Environment.GetEnvironmentVariable("TEST_IN_MEMORY") == "true";

if (environment == "Development" && !useInMemory)
{
    await app.Services.CreateScope().ServiceProvider
        .GetRequiredService<PublishingHouseDbContext>().Database.MigrateAsync();
}

app.Run();

// Needed for tests
namespace PublishingHouse.Api
{
    public partial class Program
    {
    }
}
