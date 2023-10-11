using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Published;

using static OutOfPrint.OutOfPrintEvent;
using static PublishedEvent;

public record PublishedBook(PositiveInt TotalCopies, NonNegativeNumber TotalSoldCopies): Book
{
    public Ratio UnsoldCopiesRatio =>
        new((TotalCopies.Value - TotalSoldCopies.Value) / (decimal)TotalCopies.Value);

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
            _ => Initial,
        };

    public static readonly PublishedBook Initial =
        new PublishedBook(
            // TODO: Add methods to set sold copies
            PositiveInt.Empty,
            NonNegativeNumber.Empty
        );
}

public abstract record PublishedEvent: BookEvent
{
    public record Published(PositiveInt TotalCopies): PublishedEvent;
}
