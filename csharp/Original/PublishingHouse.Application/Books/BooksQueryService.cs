using PublishingHouse.Application.Books.DTOs;
using PublishingHouse.Persistence;

namespace PublishingHouse.Application.Books;

public class BooksQueryService: IBookQueryService
{
    public Task<BookDetails?> GetDetailsById(Guid bookId) =>
        Task.FromResult(BooksContext.Books[bookId] as BookDetails);
}
