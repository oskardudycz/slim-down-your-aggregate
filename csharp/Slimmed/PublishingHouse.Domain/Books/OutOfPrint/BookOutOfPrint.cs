namespace PublishingHouse.Books.OutOfPrint;

using static OutOfPrintEvent;

public record BookOutOfPrint: Book
{
    public static BookOutOfPrint Evolve(BookOutOfPrint book, OutOfPrintEvent @event) =>
        @event switch
        {
            MovedToOutOfPrint => new BookOutOfPrint(),
            _ => book
        };
}

public abstract record OutOfPrintEvent: BookEvent
{
    public record MovedToOutOfPrint: OutOfPrintEvent;
}
