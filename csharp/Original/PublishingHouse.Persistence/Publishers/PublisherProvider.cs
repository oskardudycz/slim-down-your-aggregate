using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Publishers;
using PublishingHouse.Persistence.Books.Mappers;

namespace PublishingHouse.Persistence.Publishers;

public class PublisherProvider: IPublisherProvider
{
    public async Task<Publisher> GetById(PublisherId publisherId, CancellationToken ct) =>
        (await dbContext.Publishers.FindAsync(new object[] { publisherId.Value }, ct))?.Map() ??
        throw new InvalidOperationException();

    private readonly PublishingHouseDbContext dbContext;

    public PublisherProvider(PublishingHouseDbContext dbContext) =>
        this.dbContext = dbContext;
}
