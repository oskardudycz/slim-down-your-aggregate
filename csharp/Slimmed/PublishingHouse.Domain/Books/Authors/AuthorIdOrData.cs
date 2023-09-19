using PublishingHouse.Books.Entities;
using PublishingHouse.Core.Validation;

namespace PublishingHouse.Books.Authors;

public record AuthorIdOrData
{
    public AuthorId? AuthorId { get; }
    public AuthorFirstName? FirstName { get; }
    public AuthorLastName? LastName { get; }

    public AuthorIdOrData(AuthorId? authorId, AuthorFirstName? firstName, AuthorLastName? lastName)
    {
        if (authorId == null)
        {
            firstName.AssertNotNull();
            lastName.AssertNotNull();
        }

        AuthorId = authorId;
        FirstName = firstName;
        LastName = lastName;
    }

    public (AuthorFirstName, AuthorLastName) Data => (FirstName.AssertNotNull(), LastName.AssertNotNull());
}
