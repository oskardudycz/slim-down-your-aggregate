namespace PublishingHouse.Books.OutOfPrint;

using static OutOfPrintEvent;

public record BookOutOfPrint: Book
{
    public static BookOutOfPrint Evolve(BookOutOfPrint book, OutOfPrintEvent @event) =>
        @event switch
        {
            MovedToOutOfPrint => new BookOutOfPrint(),
            _ => Default
        };

    public static readonly BookOutOfPrint Default = new();
}

public abstract record OutOfPrintEvent: BookEvent
{
    public record MovedToOutOfPrint: OutOfPrintEvent;
}
