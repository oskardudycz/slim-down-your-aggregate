using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Services;

public interface IPublishingHouse
{
    bool IsGenreLimitReached(Genre genre);
}


public class DummyPublishingHouse: IPublishingHouse
{
    public bool IsGenreLimitReached(Genre genre) => false;
}
