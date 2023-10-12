using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Published;

using static OutOfPrint.OutOfPrintEvent;
using static PublishedEvent;

public abstract record PublishedCommand: BookCommand
{
    public record MoveToOutOfPrint(BookId BookId): BookCommand;
}

public static class PublishedDecider
{
    public static MovedToOutOfPrint MoveToOutOfPrint(PublishedBook state, Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint)
    {
        if (state.UnsoldCopiesRatio.CompareTo(maxAllowedUnsoldCopiesRatioToGoOutOfPrint) > 0)
            throw new InvalidOperationException(
                "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

        return new MovedToOutOfPrint();
    }

    public static PublishedBook Evolve(PublishedBook state, PublishedEvent @event) =>
        @event switch
        {
            Published published =>
                new PublishedBook(
                    published.TotalCopies,
                    // TODO: Add methods to set sold copies
                    new NonNegativeNumber(0)
                ),
            _ => state,
        };
}
