using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.InPrint;

using static BookEvent;
using static BookEvent.InPrintEvent;
using static BookEvent.PublishedEvent;

public record BookInPrint(BookId Id): Book(Id)
{
    public Published MoveToPublished() =>
        new Published(Id);

    public static BookInPrint Evolve(BookInPrint book, InPrintEvent @event) =>
        @event switch
        {
            MovedToPrinting movedToPrinting =>
                new BookInPrint(
                    movedToPrinting.BookId
                ),

            _ => book
        };
}
