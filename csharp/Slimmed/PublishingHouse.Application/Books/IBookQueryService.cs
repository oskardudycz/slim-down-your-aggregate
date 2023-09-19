using PublishingHouse.Books.DTOs;
using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books;

public interface IBookQueryService
{
    Task<BookDetails?> FindDetailsById(BookId bookId, CancellationToken ct);
}
