using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishingHouse.Application.Books;
using PublishingHouse.Books;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Persistence.Books.Repositories;

namespace PublishingHouse.Application;

public static class Config
{
    public static IServiceCollection AddPublishingHouse(this IServiceCollection services, IConfiguration config) =>
        services
            .AddScoped<IBooksFactory, Book.Factory>()
            .AddScoped<IBooksRepository, BooksRepository>()
            .AddScoped<IBooksQueryRepository, BooksQueryRepository>()
            .AddScoped<IBooksService, BooksService>()
            .AddScoped<IBookQueryService, BooksQueryService>();
}
