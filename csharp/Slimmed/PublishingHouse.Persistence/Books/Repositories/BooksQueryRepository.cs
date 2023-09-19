using Microsoft.EntityFrameworkCore;
using PublishingHouse.Books.DTOs;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Persistence.Books.Mappers;

namespace PublishingHouse.Persistence.Books.Repositories;

public class BooksQueryRepository: IBooksQueryRepository
{
    public Task<BookDetails?> FindDetailsById(BookId bookId, CancellationToken ct) =>
        dbContext.Books
            .AsNoTracking()
            .Include(e => e.Author)
            .Include(e => e.Publisher)
            .Include(e => e.Reviewers)
            .Include(e => e.Chapters)
            .Include(e => e.Translations)
            .Include(e => e.Formats)
            .Where(e => e.Id == bookId.Value)
            .Select(b => b.MapToDetails())
            .SingleOrDefaultAsync(ct);

    private readonly PublishingHouseDbContext dbContext;

    public BooksQueryRepository(PublishingHouseDbContext dbContext) =>
        this.dbContext = dbContext;
}
