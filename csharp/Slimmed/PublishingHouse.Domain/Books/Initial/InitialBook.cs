using PublishingHouse.Books.Draft;
using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Initial;

using static DraftEvent;

public record InitialBook: Book
{
    public DraftCreated CreateDraft(
        Title title,
        Author author,
        Publisher publisher,
        PositiveInt edition,
        Genre? genre
    ) =>
        new DraftCreated(title, author, publisher, edition, genre);
}
