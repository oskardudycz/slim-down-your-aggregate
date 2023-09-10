using PublishingHouse.Books.Entities;

namespace PublishingHouse.Books.Authors;

public interface IAuthorProvider
{
    Task<Author> GetOrCreate(AuthorIdOrData authorIdOrData);
}
