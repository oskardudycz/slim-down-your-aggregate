using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.OutOfPrint;

using static PublishingHouse.Books.BookEvent.DraftEvent;
using static PublishingHouse.Books.BookEvent.UnderEditingEvent;
using static PublishingHouse.Books.BookEvent.InPrintEvent;
using static PublishingHouse.Books.BookEvent.PublishedEvent;
using static PublishingHouse.Books.BookEvent.OutOfPrintEvent;

public class BookOutOfPrint: Book
{
    public BookOutOfPrint(BookId bookId): base(bookId)
    {
    }

    public static BookOutOfPrint Evolve(BookOutOfPrint book, BookEvent.OutOfPrintEvent @event) =>
        @event switch
        {
            MovedToOutOfPrint draftCreated =>
                new BookOutOfPrint(draftCreated.BookId),

            _ => book
        };
}
