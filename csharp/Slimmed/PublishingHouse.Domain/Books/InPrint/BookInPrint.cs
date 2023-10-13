using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.InPrint;

using static InPrintEvent;

public record BookInPrint(PositiveInt TotalCopies): Book
{
    public static BookInPrint Evolve(BookInPrint book, InPrintEvent @event) =>
        @event switch
        {
            MovedToPrinting movedToPrinting => new BookInPrint(movedToPrinting.TotalCopies),
            _ => Initial
        };

    public static readonly BookInPrint Initial = new(PositiveInt.Empty);
}

public abstract record InPrintEvent: BookEvent
{
    public record MovedToPrinting(PositiveInt TotalCopies): InPrintEvent;
}
