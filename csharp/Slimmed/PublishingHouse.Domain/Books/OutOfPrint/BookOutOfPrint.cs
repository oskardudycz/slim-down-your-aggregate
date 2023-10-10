using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.OutOfPrint;

using static BookEvent.OutOfPrintEvent;

public record BookOutOfPrint(BookId Id): Book(Id)
{
    public static BookOutOfPrint Evolve(BookOutOfPrint book, BookEvent.OutOfPrintEvent @event) =>
        @event switch
        {
            MovedToOutOfPrint draftCreated =>
                new BookOutOfPrint(draftCreated.BookId),

            _ => book
        };
}
