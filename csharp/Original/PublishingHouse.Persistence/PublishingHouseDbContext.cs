using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PublishingHouse.Persistence.Books;
using PublishingHouse.Persistence.Books.Entities;
using PublishingHouse.Persistence.Languages;
using PublishingHouse.Persistence.Publishers;
using PublishingHouse.Persistence.Reviewers;
using PublishingHouse.Persistence.Translators;

namespace PublishingHouse.Persistence;

public class PublishingHouseDbContext: DbContext
{
    public DbSet<BookEntity> Books { get; set; } = default!;
    public DbSet<ChapterEntity> BookChapters { get; set; } = default!;
    public DbSet<LanguageEntity> Authors { get; set; } = default!;
    public DbSet<LanguageEntity> Languages { get; set; } = default!;
    public DbSet<PublisherEntity> Publishers { get; set; } = default!;
    public DbSet<ReviewerEntity> Reviewers { get; set; } = default!;
    public DbSet<TranslatorEntity> Translators { get; set; } = default!;

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
