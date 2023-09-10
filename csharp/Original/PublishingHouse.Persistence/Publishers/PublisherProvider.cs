using PublishingHouse.Books.Entities;
using PublishingHouse.Publishers;

namespace PublishingHouse.Persistence.Publishers;

public class PublisherProvider: IPublisherProvider
{
    public Task<Publisher> GetById(PublisherId publisherId)
    {
        // TODO: Get it from DB Context
        return Task.FromResult(new Publisher(publisherId, new PublisherName("Readers Digest")));
    }
}
