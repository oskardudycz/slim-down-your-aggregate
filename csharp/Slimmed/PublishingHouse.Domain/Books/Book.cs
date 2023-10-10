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
using static PublishingHouse.Books.BookEvent.UnderEditingEvent;
using static PublishingHouse.Books.BookEvent.InPrintEvent;
using static PublishingHouse.Books.BookEvent.OutOfPrintEvent;

namespace PublishingHouse.Books;

public abstract record Book
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public BookId Id { get; }

    public static Book Evolve(Book book, BookEvent @event) =>
        @event switch
        {
            DraftEvent draftEvent => book is InitialBook
                ? BookDraft.Evolve(new BookDraft(book.Id, null, new List<ChapterTitle>()), draftEvent)
                : book,

            MovedToEditing movedToEditing => book is BookDraft
                ? BookUnderEditing.Evolve(
                    new BookUnderEditing(
                        book.Id, null, false, false, new List<ReviewerId>(),
                        new List<LanguageId>(), new List<FormatType>()
                    ),
                    movedToEditing)
                : book,

            UnderEditingEvent underEditingEvent => book is BookUnderEditing underEditing
                ? BookUnderEditing.Evolve(underEditing, underEditingEvent)
                : book,
            MovedToPrinting movedToPrinting => book is BookUnderEditing
                // TODO: Add methods to set total items per format
                ? BookInPrint.Evolve(new BookInPrint(movedToPrinting.BookId), movedToPrinting)
                : book,
            PublishedEvent.Published published => book is BookInPrint
                // TODO: Add methods to set sold copies
                ? PublishedBook.Evolve(
                    new PublishedBook(book.Id, new PositiveInt(1), new PositiveInt(1)),
                    published
                )
                : book,
            MovedToOutOfPrint movedToOutOfPrint => book is PublishedBook
                ? BookOutOfPrint.Evolve(new BookOutOfPrint(movedToOutOfPrint.BookId), movedToOutOfPrint)
                : book,
            _ => book
        };

    protected Book(BookId bookId) =>
        Id = bookId;

    public class Factory: IBookFactory
    {
        public Book Create(
            BookId bookId,
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
                    new BookDraft(bookId, genre, chapters.Select(ch => ch.Title).ToList()),
                State.Editing =>
                    new BookUnderEditing(
                        bookId,
                        genre,
                        isbn != null,
                        committeeApproval != null,
                        reviewers.Select(r => r.Id).ToList(),
                        translations.Select(t => t.Language.Id).ToList(),
                        formats.Select(f => f.FormatType).ToList()
                    ),
                State.Printing =>
                    new BookInPrint(bookId),
                State.Published =>
                    new PublishedBook(
                        bookId,
                        new PositiveInt(formats.Sum(f => f.TotalCopies.Value)),
                        new PositiveInt(formats.Sum(f => f.SoldCopies.Value))
                    ),
                State.OutOfPrint =>
                    new BookOutOfPrint(bookId),
                _ => throw new InvalidOperationException()
            };
    }
}
