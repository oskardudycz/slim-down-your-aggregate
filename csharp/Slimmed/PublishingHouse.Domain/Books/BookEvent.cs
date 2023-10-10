using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books;

public abstract record BookEvent
{





    public abstract record PublishedEvent: BookEvent
    {
        public record Published: PublishedEvent;
    }
}

public abstract record BookExternalEvent
{
    public record Published(
        BookId BookId,
        ISBN ISBN,
        Title Title,
        AuthorId AuthorId
    ): BookExternalEvent;

    private BookExternalEvent() { }
}
