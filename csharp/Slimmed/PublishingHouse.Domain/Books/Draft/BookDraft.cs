using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Draft;

using static DraftEvent;
using static UnderEditing.UnderEditingEvent;

public record InitialBook: Book
{
    public static InitialBook Initial = new();
}

public record BookDraft(Genre? Genre, IReadOnlyList<ChapterTitle> ChapterTitles): Book
{
    public int ChaptersCount => ChapterTitles.Count;

    public static BookDraft Evolve(BookDraft book, DraftEvent @event) =>
        @event switch
        {
            DraftCreated draftCreated =>
                new BookDraft(
                    draftCreated.Genre,
                    new List<ChapterTitle>()
                ),

            ChapterAdded chapterAdded =>
                new BookDraft(
                    book.Genre,
                    book.ChapterTitles.Union(new[] { chapterAdded.Chapter.Title }).ToList()
                ),
            _ => book
        };

    public static readonly BookDraft Default =
        new(null, new List<ChapterTitle>());
}

public abstract record DraftEvent: BookEvent
{
    public record DraftCreated(
        Title Title,
        Author Author,
        Publisher Publisher,
        PositiveInt Edition,
        Genre? Genre
    ): DraftEvent;

    public record ChapterAdded(Chapter Chapter): DraftEvent;
}
