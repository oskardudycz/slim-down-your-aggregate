using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;
using static PublishingHouse.Books.BookEvent.DraftEvent;

namespace PublishingHouse.Books.Initial;

public class InitialBook: Book
{
    public InitialBook(BookId bookId): base(bookId) { }

    public DraftCreated CreateDraft(
        Title title,
        Author author,
        Publisher publisher,
        PositiveInt edition,
        Genre? genre
    ) =>
        new DraftCreated(Id, title, author, publisher, edition, genre);
}
