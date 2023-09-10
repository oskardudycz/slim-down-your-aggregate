using PublishingHouse.Books.Entities;

namespace PublishingHouse.Authors;

public interface IAuthorProvider
{
    Task<Author> GetOrCreate(AuthorIdOrData authorIdOrData);
}
