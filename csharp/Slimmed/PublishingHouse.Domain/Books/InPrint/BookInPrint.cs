namespace PublishingHouse.Books.InPrint;

using static InPrintEvent;
using static BookEvent.PublishedEvent;

public record BookInPrint: Book
{
    public Published MoveToPublished() =>
        new Published();

    public static BookInPrint Evolve(BookInPrint book, InPrintEvent @event) =>
        @event switch
        {
            MovedToPrinting => new BookInPrint(),

            _ => book
        };
}
public abstract record InPrintEvent: BookEvent
{
    public record MovedToPrinting: InPrintEvent;
}
