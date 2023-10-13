using PublishingHouse.Books.Entities;
using PublishingHouse.Persistence.Books.DTOs;

namespace PublishingHouse.Persistence.Books.Repositories;

public interface IBooksQueryRepository
{
    Task<BookDetails?> FindDetailsById(BookId bookId, CancellationToken ct);
}
