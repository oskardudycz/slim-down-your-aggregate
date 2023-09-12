using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public record Author(AuthorId Id, AuthorFirstName FirstName, AuthorLastName LastName);
public record AuthorId(Guid Value): NonEmptyGuid(Value);
public record AuthorFirstName(string Value) : NonEmptyString(Value);
public record AuthorLastName(string Value) : NonEmptyString(Value);
