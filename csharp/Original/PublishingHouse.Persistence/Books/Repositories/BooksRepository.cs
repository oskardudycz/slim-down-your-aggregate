using Microsoft.EntityFrameworkCore;
using PublishingHouse.Books;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Persistence.Books.Mappers;

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
            .AsNoTracking()
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
        dbContext.Books.Add(book.MapToEntity(dbContext));

        return dbContext.SaveChangesAsync(ct);
    }

    public async Task Update(Book book, CancellationToken ct)
    {
        var local = await dbContext.Books.FindAsync(
            new object?[] { book.Id.Value },
            cancellationToken: ct
            ) ?? throw new InvalidOperationException();

        book.MapTo(local);

        await dbContext.SaveChangesAsync(ct);
    }
}
