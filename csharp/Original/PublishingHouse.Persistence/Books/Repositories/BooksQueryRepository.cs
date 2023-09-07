using PublishingHouse.Books.DTOs;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Persistence.Books.Mappers;

namespace PublishingHouse.Persistence.Books.Repositories;

public class BooksQueryRepository: IBooksQueryRepository
{
    public async Task<BookDetails?> FindDetailsById(BookId bookId)
    {
        var book = await PublishingHouseContext.Find(bookId.Value);

        return book?.MapToDetails();
    }
}
