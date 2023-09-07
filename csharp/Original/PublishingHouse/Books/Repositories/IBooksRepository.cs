using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Repositories;

public interface IBooksRepository
{
    Task<Book?> FindById(BookId bookId);

    Task Add(Book book);
}
