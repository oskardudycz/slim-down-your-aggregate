using Microsoft.EntityFrameworkCore;
using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Entities;
using PublishingHouse.Persistence.Books.Mappers;

namespace PublishingHouse.Persistence.Authors;

public class AuthorProvider: IAuthorProvider
{
    public async Task<Author> GetOrCreate(AuthorIdOrData authorIdOrData, CancellationToken ct)
    {
        if (authorIdOrData.AuthorId != null)
            return (await dbContext.Authors.AsNoTracking()
                       .SingleOrDefaultAsync(a => a.Id == authorIdOrData.AuthorId.Value, cancellationToken: ct))
                   ?.Map() ??
                   throw new InvalidOperationException();

        var (firstName, lastName) = authorIdOrData.Data;

        var author = new AuthorEntity { FirstName = firstName.Value, LastName = lastName.Value };

        dbContext.Authors.Add(author);
        await dbContext.SaveChangesAsync(ct);

        return author.Map();
    }

    private readonly PublishingHouseDbContext dbContext;

    public AuthorProvider(PublishingHouseDbContext dbContext) =>
        this.dbContext = dbContext;
}
