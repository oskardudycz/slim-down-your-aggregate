using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record SetISBNCommand(BookId BookId, ISBN ISBN);
