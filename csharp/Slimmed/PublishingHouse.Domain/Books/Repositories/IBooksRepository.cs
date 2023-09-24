using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Repositories;

public interface IBooksRepository
{
    Task<Book?> FindById(BookId bookId, CancellationToken ct);

    Task Store(Book book, CancellationToken ct);
}
