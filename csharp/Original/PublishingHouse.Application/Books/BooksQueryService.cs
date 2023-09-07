using PublishingHouse.Books.DTOs;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Repositories;

namespace PublishingHouse.Application.Books;

public class BooksQueryService: IBookQueryService
{
    public Task<BookDetails?> FindDetailsById(BookId bookId) =>
        repository.FindDetailsById(bookId);

    public BooksQueryService(IBooksQueryRepository repository) =>
        this.repository = repository;

    private readonly IBooksQueryRepository repository;
}
