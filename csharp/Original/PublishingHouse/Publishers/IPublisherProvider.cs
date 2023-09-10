using PublishingHouse.Books.Entities;

namespace PublishingHouse.Publishers;

public interface IPublisherProvider
{
    Task<Publisher> GetById(PublisherId publisherId);
}
