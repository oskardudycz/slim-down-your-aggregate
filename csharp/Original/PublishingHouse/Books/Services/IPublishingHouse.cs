using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Services;

public interface IPublishingHouse
{
    bool IsGenreLimitReached(Genre genre);
}

