using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Published;

using static BookEvent;
using static BookEvent.PublishedEvent;
using static OutOfPrint.OutOfPrintEvent;

public record PublishedBook: Book
{
    private readonly PositiveInt totalCopies;
    private readonly PositiveInt totalSoldCopies;

    private Ratio UnsoldCopiesRatio =>
        new((totalCopies.Value - totalSoldCopies.Value) / (decimal)totalCopies.Value);

    public PublishedBook(
        PositiveInt totalCopies,
        PositiveInt totalSoldCopies
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

    public static PublishedBook Evolve(PublishedBook book, PublishedEvent @event) =>
        @event switch
        {
            Published =>
                new PublishedBook(
                    // TODO: Add methods to set sold copies
                    new PositiveInt(1),
                    new PositiveInt(1)
                ),

            _ => book
        };
}
