using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Published;

using static PublishingHouse.Books.BookEvent;
using static PublishingHouse.Books.BookEvent.DraftEvent;
using static PublishingHouse.Books.BookEvent.UnderEditingEvent;
using static PublishingHouse.Books.BookEvent.InPrintEvent;
using static PublishingHouse.Books.BookEvent.PublishedEvent;
using static PublishingHouse.Books.BookEvent.OutOfPrintEvent;

public class PublishedBook: Book
{
    private readonly PositiveInt totalCopies;
    private readonly PositiveInt totalSoldCopies;

    private Ratio UnsoldCopiesRatio =>
        new((totalCopies.Value - totalSoldCopies.Value) / (decimal)totalCopies.Value);

    public PublishedBook(
        BookId bookId,
        PositiveInt totalCopies,
        PositiveInt totalSoldCopies
    ): base(bookId)
    {
        this.totalCopies = totalCopies;
        this.totalSoldCopies = totalSoldCopies;
    }

    public MovedToOutOfPrint MoveToOutOfPrint(Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint)
    {
        if (UnsoldCopiesRatio.CompareTo(maxAllowedUnsoldCopiesRatioToGoOutOfPrint) > 0)
            throw new InvalidOperationException(
                "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

        return new MovedToOutOfPrint(Id);
    }

    public static PublishedBook Evolve(PublishedBook book, PublishedEvent @event) =>
        @event switch
        {
            Published published =>
                new PublishedBook(
                    published.BookId,
                    // TODO: Add methods to set sold copies
                    new PositiveInt(1),
                    new PositiveInt(1)
                ),

            _ => book
        };
}
