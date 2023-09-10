using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PublishingHouse.Persistence.Authors;
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
    public DbSet<AuthorEntity> Authors { get; set; } = default!;
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
        modelBuilder.Entity<AuthorEntity>()
            .ToTable("Authors");
        modelBuilder.Entity<LanguageEntity>()
            .ToTable("Languages");
        modelBuilder.Entity<PublisherEntity>()
            .ToTable("Publishers");
        modelBuilder.Entity<ReviewerEntity>()
            .ToTable("Reviewers");
        modelBuilder.Entity<TranslatorEntity>()
            .ToTable("Translators");

        modelBuilder.Entity<BookEntity>()
            .ToTable("Books")
            .OwnsMany(b => b.Formats, a =>
            {
                a.ToTable("BookFormats");
                a.WithOwner().HasForeignKey("BookId");
                a.HasKey(p => p.FormatType);
            })
            .OwnsMany(b => b.Chapters, a =>
            {
                a.ToTable("BookChapters");
                a.WithOwner().HasForeignKey("BookId");
            })
            .OwnsMany(b => b.Translations, a =>
            {
                a.ToTable("BookTranslations");
                a.WithOwner().HasForeignKey("BookId");

                a.Navigation(d => d.Language);
                a.HasKey("BookId", "LanguageId");
            });

        modelBuilder.Entity<BookEntity>().OwnsOne(
            o => o.CommitteeApproval,
            sa =>
            {
                sa.Property(p => p.Feedback);
                sa.Property(p => p.IsApproved);
            });
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
