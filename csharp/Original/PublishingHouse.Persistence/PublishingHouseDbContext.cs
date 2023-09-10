using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PublishingHouse.Persistence;

public class PublishingHouseDbContext: DbContext
{
    public PublishingHouseDbContext(DbContextOptions<PublishingHouseDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.SetupProductsModel();
    }
}

public class PublishingHouseDbContextFactory: IDesignTimeDbContextFactory<PublishingHouseDbContext>
{
    public PublishingHouseDbContext CreateDbContext(params string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PublishingHouseDbContext>();

        if (optionsBuilder.IsConfigured)
            return new PublishingHouseDbContext(optionsBuilder.Options);

        //Called by parameterless ctor Usually Migrations
        var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";

        var connectionString =
            new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build()
                .GetConnectionString("WarehouseDB");

        optionsBuilder.UseNpgsql(connectionString);

        return new PublishingHouseDbContext(optionsBuilder.Options);
    }

    public static PublishingHouseDbContext Create()
        => new PublishingHouseDbContextFactory().CreateDbContext();
}
