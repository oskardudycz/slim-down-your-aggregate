using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Published;

namespace PublishingHouse.Books.InPrint;

using static PublishedEvent;
using static InPrintCommand;

public abstract record InPrintCommand: BookCommand
{
    public record MoveToPublished(BookId BookId): InPrintCommand;

    private InPrintCommand(){}
}

public static class InPrintDecider
{
    public static Published MoveToPublished(MoveToPublished command, BookInPrint state) =>
        new Published(state.TotalCopies);
}
