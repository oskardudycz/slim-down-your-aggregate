using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublishingHouse.Persistence;

namespace PublishingHouse.Api.Tests;

public class ApiSpecification: ApiSpecification<Program>
{
    public ApiSpecification(): base(new TestWebApplicationFactory()) { }

    private ApiSpecification(string schemaName): base(new TestWebApplicationFactory(schemaName)) { }

    public static ApiSpecification WithSchema(string schemaName) => new(schemaName);
}

public class TestWebApplicationFactory: WebApplicationFactory<Program>
{
    private readonly string schemaName;

    public TestWebApplicationFactory(): this(Guid.NewGuid().ToString("N").ToLower()) { }

    public TestWebApplicationFactory(string schemaName) =>
        this.schemaName = schemaName;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddTransient(s =>
                {
                    var options = new DbContextOptionsBuilder<PublishingHouseDbContext>();

                    if (Environment.GetEnvironmentVariable("TEST_IN_MEMORY") == "true")
                    {
                        return options
                            .UseInMemoryDatabase(schemaName)
                            .Options;
                    }

                    var connectionString = s.GetRequiredService<IConfiguration>()
                        .GetConnectionString("PublishingHouse");

                    options.UseNpgsql(
                        $"{connectionString}; searchpath = {schemaName.ToLower()}",
                        x => x.MigrationsHistoryTable("__EFMigrationsHistory", schemaName.ToLower()));
                    return options.Options;
                });
        });

        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<PublishingHouseDbContext>().Database;
        database.EnsureCreated();

        if (Environment.GetEnvironmentVariable("TEST_IN_MEMORY") != "true")
        {
            database.ExecuteSqlRaw("TRUNCATE TABLE \"Books\" CASCADE");
            database.ExecuteSqlRaw("TRUNCATE TABLE \"Authors\" CASCADE");
        }

        return host;
    }
}
