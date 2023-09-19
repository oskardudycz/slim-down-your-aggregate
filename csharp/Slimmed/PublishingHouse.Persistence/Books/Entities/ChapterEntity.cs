using PublishingHouse.Books.Entities;

namespace PublishingHouse.Persistence.Books.Entities;

public class ChapterEntity
{
    public required int Number { get; set; }
    public required string Title { get; set; }
    public string Content { get; set; } = string.Empty;

    public ChapterEntity Update(Chapter chapter)
    {
        if (Title != chapter.Title.Value)
            Title = chapter.Title.Value;
        if (Content != chapter.Content.Value)
            Content = chapter.Content.Value;

        return this;
    }
}
