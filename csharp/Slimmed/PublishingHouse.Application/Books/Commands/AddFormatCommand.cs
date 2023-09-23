using PublishingHouse.Books.Entities;

namespace PublishingHouse.Application.Books.Commands;

public record AddFormatCommand(BookId BookId, Format Format);
