using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record MoveToOutOfPrintCommand(BookId BookId);
