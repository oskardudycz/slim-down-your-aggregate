using PublishingHouse.Authors;
using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Application.Books.Commands;

public record CreateDraftCommand(
    BookId BookId,
    Title Title,
    AuthorIdOrData Author,
    PublisherId PublisherId,
    PositiveInt Edition,
    Genre? Genre
);
