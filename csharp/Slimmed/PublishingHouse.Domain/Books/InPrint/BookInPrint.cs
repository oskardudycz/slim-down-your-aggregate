using PublishingHouse.Books.Published;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.InPrint;

using static InPrintEvent;
using static PublishedEvent;

public record BookInPrint(PositiveInt TotalCopies): Book
{
    public Published MoveToPublished() =>
        new Published(TotalCopies);

    public static BookInPrint Evolve(BookInPrint book, InPrintEvent @event) =>
        @event switch
        {
            MovedToPrinting movedToPrinting => new BookInPrint(movedToPrinting.TotalCopies),
            _ => Default
        };

    public static readonly BookInPrint Default = new(new PositiveInt(1));
}

public abstract record InPrintEvent: BookEvent
{
    public record MovedToPrinting(PositiveInt TotalCopies): InPrintEvent;
}
