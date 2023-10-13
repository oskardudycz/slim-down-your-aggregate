using PublishingHouse.Books;
using PublishingHouse.Books.Entities;

namespace PublishingHouse.Persistence.Books.Repositories;

public interface IBooksRepository
{
    Task GetAndUpdate(BookId id, Func<BookEntity?, BookEvent[]> handle, CancellationToken ct);
}
