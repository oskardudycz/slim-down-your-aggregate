using PublishingHouse.Books.Draft;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.OutOfPrint;
using PublishingHouse.Books.Published;
using PublishingHouse.Books.UnderEditing;
using PublishingHouse.Core.ValueObjects;
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
                ? BookDraft.Evolve(BookDraft.Default, draftEvent)
                : book,

            MovedToEditing movedToEditing => book is BookDraft
                ? BookUnderEditing.Evolve(BookUnderEditing.Initial, movedToEditing)
                : book,

            UnderEditingEvent underEditingEvent => book is BookUnderEditing underEditing
                ? BookUnderEditing.Evolve(underEditing, underEditingEvent)
                : book,

            MovedToPrinting movedToPrinting => book is BookUnderEditing
                ? BookInPrint.Evolve(BookInPrint.Initial, movedToPrinting)
                : book,

            PublishedEvent.Published published => book is BookInPrint
                ? PublishedBook.Evolve(PublishedBook.Initial, published)
                : book,

            MovedToOutOfPrint movedToOutOfPrint => book is PublishedBook
                ? BookOutOfPrint.Evolve(BookOutOfPrint.Initial, movedToOutOfPrint)
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
                        formats.Select(f => (f.FormatType, f.TotalCopies)).ToList()
                    ),
                State.Printing =>
                    new BookInPrint(new PositiveInt(formats.Sum(f => f.TotalCopies.Value))),
                State.Published =>
                    new PublishedBook(
                        new PositiveInt(formats.Sum(f => f.TotalCopies.Value)),
                        new NonNegativeNumber(formats.Sum(f => f.SoldCopies.Value))
                    ),
                State.OutOfPrint =>
                    new BookOutOfPrint(),
                _ => throw new InvalidOperationException()
            };
    }
}
