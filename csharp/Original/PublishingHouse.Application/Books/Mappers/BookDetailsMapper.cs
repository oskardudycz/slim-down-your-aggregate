using PublishingHouse.Application.Books.DTOs;
using PublishingHouse.Persistence.Books;

namespace PublishingHouse.Application.Books.Mappers;

public static class BookDetailsMapper
{
    public static BookDetails Map(this BookEntity entity) =>
        new BookDetails(entity.Id);
}
