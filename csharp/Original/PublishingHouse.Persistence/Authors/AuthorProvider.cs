using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Entities;
using PublishingHouse.Core.Validation;

namespace PublishingHouse.Persistence.Authors;

public class AuthorProvider: IAuthorProvider
{
    public Task<Author> GetOrCreate(AuthorIdOrData authorIdOrData)
    {
        // TODO: Get it from DB Context

        if (authorIdOrData.AuthorId != null)
            return Task.FromResult(new Author(authorIdOrData.AuthorId, new AuthorFirstName("George"), new AuthorLastName("Orwell")));

        var (firstName, lastName) = authorIdOrData.Data;

        return Task.FromResult(new Author(AuthorId.Generate(), firstName, lastName));
    }
}
