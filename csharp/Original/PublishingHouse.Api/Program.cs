using Microsoft.EntityFrameworkCore;
using PublishingHouse.Application;
using PublishingHouse.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRouting()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPublishingHouse(builder.Configuration)
    .AddControllers();

var app = builder.Build();

app.UseRouting()
    .UseRouting()
    .UseAuthorization()
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

if (environment == "Development")
{
    await app.Services.CreateScope().ServiceProvider
        .GetRequiredService<PublishingHouseDbContext>().Database.MigrateAsync();
}

app.Run();

// Needed for tests
public partial class Program {}
