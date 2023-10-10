using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishingHouse.Application.Books;
using PublishingHouse.Books;
using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Publishers;
using PublishingHouse.Core.ValueObjects;
using PublishingHouse.Persistence;
using PublishingHouse.Persistence.Authors;
using PublishingHouse.Persistence.Books.Repositories;
using PublishingHouse.Persistence.Publishers;

namespace PublishingHouse.Application;

public static class Config
{
    public static IServiceCollection AddPublishingHouse(this IServiceCollection services, IConfiguration config) =>
        services
            .AddScoped<IBookFactory, Book.Factory>()
            .AddScoped<IBooksRepository, BooksRepository>()
            .AddScoped<IBooksQueryRepository, BooksQueryRepository>()
            .AddScoped<IBooksService, BooksService>(sp =>
                new BooksService(
                    sp.GetRequiredService<IBooksRepository>(),
                    sp.GetRequiredService<IBookFactory>(),
                    sp.GetRequiredService<IAuthorProvider>(),
                    sp.GetRequiredService<IPublisherProvider>(),
                    //TODO: Get this from config or environement variable
                    new PositiveInt(3),
                    new PositiveInt(5),
                    new Ratio(0.1m)
                ))
            .AddScoped<IBookQueryService, BooksQueryService>()
            .AddScoped<IAuthorProvider, AuthorProvider>()
            .AddScoped<IPublisherProvider, PublisherProvider>()
            .AddDbContext<PublishingHouseDbContext>();
}
