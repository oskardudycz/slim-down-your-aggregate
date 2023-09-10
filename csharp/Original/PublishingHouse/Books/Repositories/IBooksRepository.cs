using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Repositories;

public interface IBooksRepository
{
    Task<Book?> FindById(BookId bookId, CancellationToken ct);

    Task Add(Book book, CancellationToken ct);

    Task Update(Book book, CancellationToken ct);
}
