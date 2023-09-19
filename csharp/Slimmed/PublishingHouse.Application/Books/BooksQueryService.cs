using PublishingHouse.Books.DTOs;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Repositories;

namespace PublishingHouse.Application.Books;

public class BooksQueryService: IBookQueryService
{
    public Task<BookDetails?> FindDetailsById(BookId bookId, CancellationToken ct) =>
        repository.FindDetailsById(bookId, ct);

    public BooksQueryService(IBooksQueryRepository repository) =>
        this.repository = repository;

    private readonly IBooksQueryRepository repository;
}
