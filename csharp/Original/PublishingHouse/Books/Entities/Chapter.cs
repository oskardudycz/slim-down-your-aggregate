using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public class Chapter
{
    public Chapter(ChapterId id, ChapterNumber number, ChapterTitle title, ChapterContent content)
    {
        Id = id;
        Number = number;
        Title = title;
        Content = content;
    }

    public ChapterId Id { get; }
    public ChapterNumber Number { get; }
    public ChapterTitle Title { get; }
    public ChapterContent Content { get; }
}

public record ChapterId(Guid Value): NonEmptyGuid(Value)
{
    public static ChapterId Generate() => new(Guid.NewGuid());
}

public record ChapterNumber(int Value): PositiveInt(Value);


public record ChapterTitle(string Value): NonEmptyString(Value);


public record ChapterContent(string Value)
{
    public static readonly ChapterContent Empty = new(string.Empty);
};
