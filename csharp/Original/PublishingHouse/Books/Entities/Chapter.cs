using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Entities;

public class Chapter
{
    public Chapter(ChapterNumber number, ChapterTitle title, ChapterContent content)
    {
        Number = number;
        Title = title;
        Content = content;
    }
    public ChapterNumber Number { get; }
    public ChapterTitle Title { get; }
    public ChapterContent Content { get; }
}

public record ChapterNumber(int Value): PositiveInt(Value);


public record ChapterTitle(string Value): NonEmptyString(Value);


public record ChapterContent(string Value)
{
    public static readonly ChapterContent Empty = new(string.Empty);
};
