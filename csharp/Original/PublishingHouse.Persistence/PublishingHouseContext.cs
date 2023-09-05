using PublishingHouse.Persistence.Books;

namespace PublishingHouse.Persistence;

public static class PublishingHouseContext
{
    private static Dictionary<Guid, BookEntity> books = new();

    public static BookEntity? Find(Guid id) =>
        books.TryGetValue(id, out var value) ? value : null;

    public static void Add(BookEntity entity) =>
        books.Add(entity.Id, entity);
}
