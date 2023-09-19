using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record Genre(string Value): NonEmptyString(Value);

