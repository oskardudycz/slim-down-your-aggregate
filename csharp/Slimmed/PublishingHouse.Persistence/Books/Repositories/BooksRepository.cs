using Microsoft.EntityFrameworkCore;
using PublishingHouse.Books;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Core.Validation;
using PublishingHouse.Persistence.Books.Entities;
using PublishingHouse.Persistence.Books.Mappers;
using PublishingHouse.Persistence.Books.ValueObjects;
using PublishingHouse.Persistence.Core.Repositories;
using PublishingHouse.Persistence.Reviewers;
using static PublishingHouse.Books.BookEvent;

namespace PublishingHouse.Persistence.Books.Repositories;

public class BooksRepository:
    EntityFrameworkRepository<BookEntity, BookId, BookEvent, PublishingHouseDbContext>, IBooksRepository
{
    private readonly IBookFactory bookFactory;

    public BooksRepository(PublishingHouseDbContext dbContext, IBookFactory bookFactory): base(
        dbContext,
        id => e => e.Id == id.Value
    ) =>
        this.bookFactory = bookFactory;

    protected override IQueryable<BookEntity> Includes(DbSet<BookEntity> query) =>
        query.Include(e => e.Author)
            .Include(e => e.Publisher)
            .Include(e => e.Reviewers)
            .Include(e => e.Chapters)
            .Include(e => e.Translations)
            .Include(e => e.Formats);

    protected override void Evolve(PublishingHouseDbContext dbContext, BookEntity? current, BookEvent @event)
    {
        switch (@event)
        {
            case DraftCreated(var bookId, var title, var author, var publisher, var positiveInt, var genre):
            {
                dbContext.Books.Add(new BookEntity
                {
                    Id = bookId.Value,
                    CurrentState = BookEntity.State.Writing,
                    Title = title.Value,
                    AuthorId = author.Id.Value,
                    PublisherId = publisher.Id.Value,
                    Edition = positiveInt.Value,
                    Genre = genre?.Value,
                    Chapters = new List<ChapterEntity>(),
                    Formats = new List<FormatEntity>(),
                    Reviewers = new List<ReviewerEntity>(),
                    Translations = new List<TranslationVO>(),
                    Version = 0
                });
                break;
            }
            case ChapterAdded(_, var chapter):
            {
                current.AssertNotNull()
                    .Chapters
                    .Add(new ChapterEntity
                    {
                        Number = chapter.Number.Value, Title = chapter.Title.Value, Content = chapter.Content.Value
                    });
                break;
            }
            case FormatAdded(_, var format):
            {
                current.AssertNotNull()
                    .Formats
                    .Add(new FormatEntity
                    {
                        FormatType = format.FormatType.Value,
                        SoldCopies = format.SoldCopies.Value,
                        TotalCopies = format.TotalCopies.Value
                    });
                break;
            }
            case FormatRemoved(_, var format):
            {
                var formatToRemove = current.AssertNotNull()
                    .Formats
                    .Single(f => f.FormatType == format.FormatType.Value);
                current.Formats.Remove(formatToRemove);
                break;
            }
            case TranslationAdded(_, var (language, translator)):
            {
                current.AssertNotNull().Translations.Add(new TranslationVO
                {
                    LanguageId = language.Id.Value, TranslatorId = translator.Id.Value
                });
                break;
            }
            case TranslationRemoved(_, var (language, translator)):
            {
                var translationToRemove = current.AssertNotNull()
                    .Translations
                    .Single(t => t.TranslatorId == translator.Id.Value && t.LanguageId == language.Id.Value);

                current.Translations.Remove(translationToRemove);
                break;
            }
            case ReviewerAdded(_, var reviewer):
            {
                current.AssertNotNull()
                    .Reviewers
                    .Add(new ReviewerEntity { Id = reviewer.Id.Value, Name = reviewer.Name.Value });
                break;
            }
            case MovedToEditing _:
            {
                current.AssertNotNull()
                    .CurrentState = BookEntity.State.Editing;
                break;
            }
            case Approved (_, var committeeApproval):
            {
                current.AssertNotNull()
                        .CommitteeApproval =
                    new CommitteeApprovalVO(committeeApproval.IsApproved, committeeApproval.Feedback.Value);
                break;
            }
            case ISBNSet (_, var isbn):
            {
                current.AssertNotNull()
                    .ISBN = isbn.Value;
                break;
            }
            case MovedToPrinting _:
            {
                current.AssertNotNull()
                    .CurrentState = BookEntity.State.Printing;
                break;
            }
            case Published _:
            {
                current.AssertNotNull()
                    .CurrentState = BookEntity.State.Published;
                break;
            }
            case MovedToOutOfPrint _:
            {
                current.AssertNotNull()
                    .CurrentState = BookEntity.State.OutOfPrint;
                break;
            }
            default:
                throw new InvalidOperationException();
        }

        if (current != null)
            current.Version++;
    }

    protected override object Enrich(BookEvent @event, BookEntity? current)
    {
        if (@event is Published published && current != null)
        {
            return new BookExternalEvent.Published(
                published.BookId,
                new ISBN(current.ISBN!),
                new Title(current.Title),
                new AuthorId(current.Author.Id)
            );
        }

        return base.Enrich(@event, current);
    }
}
