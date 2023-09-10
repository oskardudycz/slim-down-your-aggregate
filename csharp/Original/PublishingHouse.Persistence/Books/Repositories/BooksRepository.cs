using Microsoft.EntityFrameworkCore;
using PublishingHouse.Books;
using PublishingHouse.Books.DTOs;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;
using PublishingHouse.Persistence.Authors;
using PublishingHouse.Persistence.Books.Entities;
using PublishingHouse.Persistence.Books.Mappers;
using PublishingHouse.Persistence.Languages;
using PublishingHouse.Persistence.Publishers;
using PublishingHouse.Persistence.Reviewers;
using PublishingHouse.Persistence.Translators;

namespace PublishingHouse.Persistence.Books.Repositories;

public class BooksRepository: IBooksRepository
{
    private readonly PublishingHouseDbContext dbContext;
    private readonly IBookFactory bookFactory;

    public BooksRepository(PublishingHouseDbContext dbContext, IBookFactory bookFactory)
    {
        this.dbContext = dbContext;
        this.bookFactory = bookFactory;
    }

    public async Task<Book?> FindById(BookId bookId, CancellationToken ct)
    {
        var book = await dbContext.Books
            .Include(e => e.Author)
            .Include(e => e.Publisher)
            .Include(e => e.Reviewers)
            .Include(e => e.Chapters)
            .Include(e => e.Translations)
            .Include(e => e.Formats)
            .Where(e => e.Id == bookId.Value)
            .SingleOrDefaultAsync(ct);

        return book?.MapToAggregate(bookFactory);
    }

    public Task Add(Book book, CancellationToken ct)
    {
        dbContext.Books.Add(book.MapToEntity());

        return dbContext.SaveChangesAsync(ct);
    }
}
