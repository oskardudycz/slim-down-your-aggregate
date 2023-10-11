using PublishingHouse.Books.Published;

namespace PublishingHouse.Books.InPrint;

using static PublishedEvent;

public static class InPrintDecider
{
    public static Published MoveToPublished(BookInPrint state) =>
        new Published(state.TotalCopies);
}
