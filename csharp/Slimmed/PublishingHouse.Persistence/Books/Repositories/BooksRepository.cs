using Microsoft.EntityFrameworkCore;
using PublishingHouse.Books;
using PublishingHouse.Books.Draft;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.OutOfPrint;
using PublishingHouse.Books.UnderEditing;
using PublishingHouse.Core.Events;
using PublishingHouse.Core.Validation;
using PublishingHouse.Persistence.Books.Entities;
using PublishingHouse.Persistence.Books.ValueObjects;
using PublishingHouse.Persistence.Core.Repositories;
using PublishingHouse.Persistence.Reviewers;
using static PublishingHouse.Books.BookEvent.PublishedEvent;

namespace PublishingHouse.Persistence.Books.Repositories;

using static DraftEvent;

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

    protected override void Evolve(
        PublishingHouseDbContext dbContext,
        BookEntity? current,
        EventEnvelope<BookEvent> eventEnvelope
    )
    {
        var @event = eventEnvelope.Event;
        var id = eventEnvelope.Metadata.RecordId.Value;

        switch (@event)
        {
            case DraftCreated(var title, var author, var publisher, var positiveInt, var genre):
            {
                dbContext.Books.Add(new BookEntity
                {
                    Id = id,
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
            case ChapterAdded(var chapter):
            {
                current.AssertNotNull()
                    .Chapters
                    .Add(new ChapterEntity
                    {
                        Number = chapter.Number.Value, Title = chapter.Title.Value, Content = chapter.Content.Value
                    });
                break;
            }
            case UnderEditingEvent.FormatAdded(var format):
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
            case UnderEditingEvent.FormatRemoved(var format):
            {
                var formatToRemove = current.AssertNotNull()
                    .Formats
                    .Single(f => f.FormatType == format.FormatType.Value);
                current.Formats.Remove(formatToRemove);
                break;
            }
            case UnderEditingEvent.TranslationAdded(var (language, translator)):
            {
                current.AssertNotNull().Translations.Add(new TranslationVO
                {
                    LanguageId = language.Id.Value, TranslatorId = translator.Id.Value
                });
                break;
            }
            case UnderEditingEvent.TranslationRemoved(var (language, translator)):
            {
                var translationToRemove = current.AssertNotNull()
                    .Translations
                    .Single(t => t.TranslatorId == translator.Id.Value && t.LanguageId == language.Id.Value);

                current.Translations.Remove(translationToRemove);
                break;
            }
            case UnderEditingEvent.ReviewerAdded(var reviewer):
            {
                current.AssertNotNull()
                    .Reviewers
                    .Add(new ReviewerEntity { Id = reviewer.Id.Value, Name = reviewer.Name.Value });
                break;
            }
            case UnderEditingEvent.MovedToEditing _:
            {
                current.AssertNotNull()
                    .CurrentState = BookEntity.State.Editing;
                break;
            }
            case UnderEditingEvent.Approved (var committeeApproval):
            {
                current.AssertNotNull()
                        .CommitteeApproval =
                    new CommitteeApprovalVO(committeeApproval.IsApproved, committeeApproval.Feedback.Value);
                break;
            }
            case UnderEditingEvent.ISBNSet (var isbn):
            {
                current.AssertNotNull()
                    .ISBN = isbn.Value;
                break;
            }
            case InPrintEvent.MovedToPrinting _:
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
            case OutOfPrintEvent.MovedToOutOfPrint _:
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

    protected override IEventEnvelope Enrich(EventEnvelope<BookEvent> eventEnvelope, BookEntity? current)
    {
        var @event = eventEnvelope.Event;
        var id = eventEnvelope.Metadata.RecordId.Value;

        if (@event is Published published && current != null)
        {
            return new EventEnvelope<BookExternalEvent.Published>(
                new BookExternalEvent.Published(
                    new BookId(id),
                    new ISBN(current.ISBN!),
                    new Title(current.Title),
                    new AuthorId(current.Author.Id)
                ),
                eventEnvelope.Metadata
            );
        }

        return base.Enrich(eventEnvelope, current);
    }
}
