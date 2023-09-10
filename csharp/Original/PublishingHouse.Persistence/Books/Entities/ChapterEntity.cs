namespace PublishingHouse.Persistence.Books.Entities;

public class ChapterEntity
{
    public ChapterEntity(Guid id, int number, string title, string content)
    {
        Id = id;
        Number = number;
        Title = title;
        Content = content;
    }

    public Guid Id { get; }
    public int Number { get; }
    public string Title { get; }
    public string Content { get; }
}
