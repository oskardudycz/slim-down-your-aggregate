using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record Title(string Value): NonEmptyString(Value);
