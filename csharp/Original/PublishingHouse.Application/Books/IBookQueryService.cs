using PublishingHouse.Application.Books.DTOs;
using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books;

public interface IBookQueryService
{
    Task<BookDetails?> GetDetailsById(BookId bookI);
}
