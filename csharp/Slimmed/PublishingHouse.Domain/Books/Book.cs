using PublishingHouse.Books.Draft;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Initial;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.OutOfPrint;
using PublishingHouse.Books.Published;
using PublishingHouse.Books.UnderEditing;
using PublishingHouse.Core.ValueObjects;
using static PublishingHouse.Books.BookEvent;
using static PublishingHouse.Books.UnderEditing.UnderEditingEvent;
using static PublishingHouse.Books.InPrint.InPrintEvent;
using static PublishingHouse.Books.OutOfPrint.OutOfPrintEvent;

namespace PublishingHouse.Books;

public abstract record Book
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public static Book Evolve(Book book, BookEvent @event) =>
        @event switch
        {
            DraftEvent draftEvent => book is InitialBook
                ? BookDraft.Evolve(new BookDraft(null, new List<ChapterTitle>()), draftEvent)
                : book,

            MovedToEditing movedToEditing => book is BookDraft
                ? BookUnderEditing.Evolve(
                    new BookUnderEditing(
                        null, false, false, new List<ReviewerId>(),
                        new List<LanguageId>(), new List<FormatType>()
                    ),
                    movedToEditing)
                : book,

            UnderEditingEvent underEditingEvent => book is BookUnderEditing underEditing
                ? Evolve(underEditing, underEditingEvent)
                : book,
            MovedToPrinting movedToPrinting => book is BookUnderEditing
                // TODO: Add methods to set total items per format
                ? BookInPrint.Evolve(new BookInPrint(), movedToPrinting)
                : book,
            PublishedEvent.Published published => book is BookInPrint
                // TODO: Add methods to set sold copies
                ? PublishedBook.Evolve(
                    new PublishedBook(new PositiveInt(1), new PositiveInt(1)),
                    published
                )
                : book,
            MovedToOutOfPrint movedToOutOfPrint => book is PublishedBook
                ? BookOutOfPrint.Evolve(new BookOutOfPrint(), movedToOutOfPrint)
                : book,
            _ => book
        };

    public class Factory: IBookFactory
    {
        public Book Create(
            State state,
            Title title,
            Author author,
            Genre? genre,
            ISBN? isbn,
            CommitteeApproval? committeeApproval,
            List<Reviewer> reviewers,
            List<Chapter> chapters,
            List<Translation> translations,
            List<Format> formats
        ) =>
            state switch
            {
                State.Writing =>
                    new BookDraft(genre, chapters.Select(ch => ch.Title).ToList()),
                State.Editing =>
                    new BookUnderEditing(
                        genre,
                        isbn != null,
                        committeeApproval != null,
                        reviewers.Select(r => r.Id).ToList(),
                        translations.Select(t => t.Language.Id).ToList(),
                        formats.Select(f => f.FormatType).ToList()
                    ),
                State.Printing =>
                    new BookInPrint(),
                State.Published =>
                    new PublishedBook(
                        new PositiveInt(formats.Sum(f => f.TotalCopies.Value)),
                        new PositiveInt(formats.Sum(f => f.SoldCopies.Value))
                    ),
                State.OutOfPrint =>
                    new BookOutOfPrint(),
                _ => throw new InvalidOperationException()
            };
    }
}
