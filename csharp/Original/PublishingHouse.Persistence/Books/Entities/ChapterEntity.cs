namespace PublishingHouse.Persistence.Books.Entities;

public class ChapterEntity
{
    public required int Number { get; set; }
    public required string Title { get; set; }
    public string Content { get; set; } = string.Empty;
}
