using PublishingHouse.Application.Books.DTOs;

namespace PublishingHouse.Application.Books;

public interface IBookQueryService
{
    Task<BookDetails?> GetDetailsById(Guid bookId);
}
