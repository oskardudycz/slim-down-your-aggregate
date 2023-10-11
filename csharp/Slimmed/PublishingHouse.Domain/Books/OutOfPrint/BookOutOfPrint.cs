namespace PublishingHouse.Books.OutOfPrint;

using static OutOfPrintEvent;

public record BookOutOfPrint: Book
{
    public static BookOutOfPrint Evolve(BookOutOfPrint book, OutOfPrintEvent @event) =>
        @event switch
        {
            MovedToOutOfPrint => new BookOutOfPrint(),
            _ => Initial
        };

    public static readonly BookOutOfPrint Initial = new();
}

public abstract record OutOfPrintEvent: BookEvent
{
    public record MovedToOutOfPrint: OutOfPrintEvent;
}
