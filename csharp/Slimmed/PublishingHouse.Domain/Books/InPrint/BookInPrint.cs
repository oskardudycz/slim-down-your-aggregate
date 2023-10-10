using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.InPrint;

using static PublishingHouse.Books.BookEvent.DraftEvent;
using static PublishingHouse.Books.BookEvent.UnderEditingEvent;
using static PublishingHouse.Books.BookEvent.InPrintEvent;
using static PublishingHouse.Books.BookEvent.PublishedEvent;
using static PublishingHouse.Books.BookEvent.OutOfPrintEvent;

public class BookInPrint: Book
{
    internal BookInPrint(BookId bookId): base(bookId)
    {
    }

    public Published MoveToPublished() =>
        new Published(Id);

    public static BookInPrint Evolve(BookInPrint book, BookEvent.InPrintEvent @event) =>
        @event switch
        {
            MovedToPrinting movedToPrinting =>
                new BookInPrint(
                    movedToPrinting.BookId
                ),

            _ => book
        };
}
