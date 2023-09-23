using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record MoveToPublishedCommand(BookId BookId);
