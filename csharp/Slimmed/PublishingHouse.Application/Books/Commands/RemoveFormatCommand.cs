using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record RemoveFormatCommand(BookId BookId, Format Format);
