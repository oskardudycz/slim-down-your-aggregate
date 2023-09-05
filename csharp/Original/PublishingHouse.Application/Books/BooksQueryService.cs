using PublishingHouse.Application.Books.DTOs;
using PublishingHouse.Application.Books.Mappers;
using PublishingHouse.Persistence;

namespace PublishingHouse.Application.Books;

public class BooksQueryService: IBookQueryService
{
    public Task<BookDetails?> GetDetailsById(Guid bookId)
    {
        var book = PublishingHouseContext.Find(bookId);

        return Task.FromResult(book != null ? BookDetailsMapper.Map(book) : null);
    }
}
