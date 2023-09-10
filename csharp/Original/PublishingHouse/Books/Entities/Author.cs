using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public class Author
{
    public Author(AuthorId id, AuthorFirstName firstName, AuthorLastName lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public AuthorId Id { get; }
    public AuthorFirstName FirstName { get; }
    public AuthorLastName LastName { get; }
}

public record AuthorId(Guid Value): NonEmptyGuid(Value)
{
    public static AuthorId Generate() => new(Guid.NewGuid());
}
public record AuthorFirstName(string Value) : NonEmptyString(Value);
public record AuthorLastName(string Value) : NonEmptyString(Value);
