using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record ISBN(string Value): NonEmptyString(Value);
