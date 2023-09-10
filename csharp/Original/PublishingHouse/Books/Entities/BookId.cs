using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record BookId(Guid Value) : NonEmptyGuid(Value);
