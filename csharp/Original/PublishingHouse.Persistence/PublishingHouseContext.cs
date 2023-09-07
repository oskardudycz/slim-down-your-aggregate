using PublishingHouse.Persistence.Books;

namespace PublishingHouse.Persistence;

internal static class PublishingHouseContext
{
    private static Dictionary<Guid, BookEntity> books = new();

    public static Task<BookEntity?> Find(Guid id) =>
        Task.FromResult(books.TryGetValue(id, out var value) ? value : null);

    public static Task Add(BookEntity entity)
    {
        books.Add(entity.Id, entity);
        return Task.CompletedTask;
    }
}
