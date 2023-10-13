using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.UnderEditing;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.Draft;

using static DraftEvent;
using static DraftCommand;
using static UnderEditingEvent;

public abstract record DraftCommand: BookCommand
{
    public record CreateDraft(
        BookId BookId,
        Title Title,
        Author Author,
        Publisher Publisher,
        PositiveInt Edition,
        Genre? Genre
    ): DraftCommand;

    public record AddChapter(
        BookId BookId,
        ChapterTitle Title,
        ChapterContent Content
    ): DraftCommand;

    public record MoveToEditing(BookId BookId): DraftCommand;

    private DraftCommand() { }
}

public static class DraftDecider
{
    public static DraftCreated CreateDraft(
        CreateDraft command,
        InitialBook state
    ) =>
        new(command.Title, command.Author, command.Publisher, command.Edition, command.Genre);

    public static ChapterAdded AddChapter(AddChapter command, BookDraft state)
    {
        var (_, title, content) = command;

        if (!title.Value.StartsWith("Chapter " + (state.ChapterTitles.Count + 1)))
            throw new InvalidOperationException(
                $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {state.ChapterTitles.Count + 1}'");

        if (state.ChapterTitles.Contains(title))
            throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

        return new ChapterAdded(new Chapter(new ChapterNumber(state.ChapterTitles.Count + 1), title, content));
    }

    public static MovedToEditing MoveToEditing(MoveToEditing command, BookDraft state)
    {
        if (state.ChaptersCount < 1)
            throw new InvalidOperationException(
                "A book must have at least one chapter to move to the Editing state.");

        if (state.Genre == null)
            throw new InvalidOperationException("Book can be moved to the editing only when genre is specified");

        return new MovedToEditing(state.Genre);
    }
}
