using PublishingHouse.Books.Entities;
using PublishingHouse.Persistence.Books.DTOs;

namespace PublishingHouse.Application.Books;

public interface IBookQueryService
{
    Task<BookDetails?> FindDetailsById(BookId bookId, CancellationToken ct);
}
