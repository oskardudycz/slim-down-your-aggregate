using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishingHouse.Application.Books;
using PublishingHouse.Authors;
using PublishingHouse.Books;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Persistence;
using PublishingHouse.Persistence.Authors;
using PublishingHouse.Persistence.Books.Repositories;
using PublishingHouse.Persistence.Publishers;
using PublishingHouse.Publishers;

namespace PublishingHouse.Application;

public static class Config
{
    public static IServiceCollection AddPublishingHouse(this IServiceCollection services, IConfiguration config) =>
        services
            .AddScoped<IBooksFactory, Book.Factory>()
            .AddScoped<IBooksRepository, BooksRepository>()
            .AddScoped<IBooksQueryRepository, BooksQueryRepository>()
            .AddScoped<IBooksService, BooksService>()
            .AddScoped<IBookQueryService, BooksQueryService>()
            .AddScoped<IAuthorProvider, AuthorProvider>()
            .AddScoped<IPublisherProvider, PublisherProvider>()
            .AddDbContext<PublishingHouseDbContext>();
}
