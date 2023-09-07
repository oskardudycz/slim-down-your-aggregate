using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record CreateDraftCommand(
    BookId BookId,
    Title Title,
    Author Author,
    Publisher Publisher,
    int Edition,
    Genre? Genre
);
