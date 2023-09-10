using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Publishers;

public interface IPublisherProvider
{
    Task<Publisher> GetById(PublisherId publisherId, CancellationToken ct);
}
