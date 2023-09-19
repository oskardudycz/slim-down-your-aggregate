using Microsoft.EntityFrameworkCore;
using PublishingHouse.Books;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Persistence.Books.Mappers;
using PublishingHouse.Persistence.Core.Repositories;

namespace PublishingHouse.Persistence.Books.Repositories;

public class BooksRepository:
    EntityFrameworkRepository<Book, BookId, BookEntity, PublishingHouseDbContext>,  IBooksRepository
{
    private readonly IBookFactory bookFactory;

    public BooksRepository(PublishingHouseDbContext dbContext, IBookFactory bookFactory) : base(dbContext) =>
        this.bookFactory = bookFactory;

    protected override IQueryable<BookEntity> Includes(DbSet<BookEntity> query) =>
        query.Include(e => e.Author)
            .Include(e => e.Publisher)
            .Include(e => e.Reviewers)
            .Include(e => e.Chapters)
            .Include(e => e.Translations)
            .Include(e => e.Formats);

    protected override Book MapToAggregate(BookEntity entity) =>
        entity.MapToAggregate(bookFactory);

    protected override BookEntity MapToEntity(Book aggregate) =>
        aggregate.MapToEntity(DbContext);

    protected override void UpdateEntity(BookEntity entity, Book aggregate) =>
        entity.UpdateFrom(aggregate);
}
