using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Published;

using static OutOfPrint.OutOfPrintEvent;
using static PublishedEvent;

public record PublishedBook: Book
{
    private readonly NonNegativeNumber totalCopies;
    private readonly NonNegativeNumber totalSoldCopies;

    private Ratio UnsoldCopiesRatio =>
        new((totalCopies.Value - totalSoldCopies.Value) / (decimal)totalCopies.Value);

    public PublishedBook(
        NonNegativeNumber totalCopies,
        NonNegativeNumber totalSoldCopies
    )
    {
        this.totalCopies = totalCopies;
        this.totalSoldCopies = totalSoldCopies;
    }

    public MovedToOutOfPrint MoveToOutOfPrint(Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint)
    {
        if (UnsoldCopiesRatio.CompareTo(maxAllowedUnsoldCopiesRatioToGoOutOfPrint) > 0)
            throw new InvalidOperationException(
                "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

        return new MovedToOutOfPrint();
    }

    public static PublishedBook Evolve(PublishedBook state, PublishedEvent @event) =>
        @event switch
        {
            Published =>
                new PublishedBook(
                    // TODO: Add methods to set sold copies
                    new NonNegativeNumber(0),
                    new NonNegativeNumber(0)
                ),
            _ => Default,
        };

    public static readonly PublishedBook Default =
        new PublishedBook(
            // TODO: Add methods to set sold copies
            new NonNegativeNumber(0),
            new NonNegativeNumber(0)
        );
}

public abstract record PublishedEvent: BookEvent
{
    public record Published(PositiveInt TotalCopies): PublishedEvent;
}
