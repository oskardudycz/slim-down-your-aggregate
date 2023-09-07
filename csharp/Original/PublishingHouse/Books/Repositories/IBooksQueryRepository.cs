using PublishingHouse.Books.DTOs;
using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Repositories;

public interface IBooksQueryRepository
{
    Task<BookDetails?> FindDetailsById(BookId bookId);
}
