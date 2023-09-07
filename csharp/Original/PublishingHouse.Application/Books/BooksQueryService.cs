using PublishingHouse.Application.Books.DTOs;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Repositories;

namespace PublishingHouse.Application.Books;

public class BooksQueryService: IBookQueryService
{
    public async Task<BookDetails?> GetDetailsById(BookId bookId)
    {
        var book = await repository.FindById(bookId);

        return book != null ? new BookDetails(book.BookId.Value) : null;
    }

    public BooksQueryService(IBooksRepository repository) =>
        this.repository = repository;

    private readonly IBooksRepository repository;
}
