using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishingHouse.Application.Books;

namespace PublishingHouse.Application;

public static class Config
{
    public static IServiceCollection AddPublishingHouse(this IServiceCollection services, IConfiguration config) =>
        services.AddScoped<IBooksService, BooksService>();
}
