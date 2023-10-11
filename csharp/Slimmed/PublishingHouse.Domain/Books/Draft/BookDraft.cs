using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Draft;

using static DraftEvent;
using static UnderEditing.UnderEditingEvent;

public record BookDraft: Book
{
    private readonly Genre? genre;
    private readonly IReadOnlyList<ChapterTitle> chapterTitles;
    private int ChaptersCount => chapterTitles.Count;

    internal BookDraft(
        Genre? genre,
        IReadOnlyList<ChapterTitle> chapterTitles
    )
    {
        this.genre = genre;
        this.chapterTitles = chapterTitles;
    }

    public static ChapterAdded AddChapter(BookDraft state, ChapterTitle title, ChapterContent content)
    {
        if (!title.Value.StartsWith("Chapter " + (state.chapterTitles.Count + 1)))
            throw new InvalidOperationException(
                $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {state.chapterTitles.Count + 1}'");

        if (state.chapterTitles.Contains(title))
            throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

        return new ChapterAdded(new Chapter(new ChapterNumber(state.chapterTitles.Count + 1), title, content));
    }

    public static MovedToEditing MoveToEditing(BookDraft state)
    {
        if (state.ChaptersCount < 1)
            throw new InvalidOperationException(
                "A book must have at least one chapter to move to the Editing state.");

        if (state.genre == null)
            throw new InvalidOperationException("Book can be moved to the editing only when genre is specified");

        return new MovedToEditing(state.genre);
    }

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
                    book.genre,
                    book.chapterTitles.Union(new[] { chapterAdded.Chapter.Title }).ToList()
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
