using SlimDownYourAggregates.Tests.Slimmed.Entities;
using static SlimDownYourAggregates.Tests.Slimmed.BookEvent;
using static SlimDownYourAggregates.Tests.Slimmed.Book;

namespace SlimDownYourAggregates.Tests.Slimmed;

using static BookCommand;

public abstract record BookCommand
{
    public record AddChapter(
        BookId BookId,
        ChapterTitle Title,
        ChapterContent Content
    ): BookCommand;

    public record AddFormat(
        BookId BookId,
        Format Format
    ): BookCommand;

    public record RemoveFormat(
        BookId BookId,
        Format Format
    ): BookCommand;

    public record AddTranslation(
        BookId BookId,
        Translation Translation
    ): BookCommand;

    public record Edit(
        BookId BookId
    ): BookCommand;

    public record Approve(
        BookId BookId,
        CommitteeApproval CommitteeApproval
    ): BookCommand;

    public record Print
    (
        BookId BookId
    ): BookCommand;

    public record Publish(
        BookId BookId,
        ISBN ISBN,
        Title Title,
        Author Author
    ): BookCommand;

    public record MoveToOutOfPrint
    (
        BookId BookId
    ): BookCommand;

    private BookCommand() { }
}

public static class BookService
{
    public static ChapterAdded AddChapter(AddChapter command, Book state)
    {
        var (_, title, content) = command;

        if (state.ChapterTitles.Any(chap => chap == title.Value))
            throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

        if (title.Value != $"Chapter {state.ChapterTitles.Count}")
            throw new InvalidOperationException(
                $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {state.ChapterTitles.Count}'");

        var chapter = new Chapter(title, content);

        return new ChapterAdded(state.BookId, chapter);
    }

    public static MovedToEditing MoveToEditing(Edit command, Book state)
    {
        if (state.CurrentState != State.Writing)
            throw new InvalidOperationException("Cannot move to Editing state from the current state.");

        if (state.ChapterTitles.Count < 1)
            throw new InvalidOperationException("A book must have at least one chapter to move to the Editing state.");

        return new MovedToEditing(state.BookId);
    }

    public static TranslationAdded AddTranslation(AddTranslation command, Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot add translation of a book that is not in the Editing state.");

        if (state.ChapterTitles.Count >= 5)
            throw new InvalidOperationException("Cannot add more translations. Maximum 5 translations are allowed.");

        return new TranslationAdded(state.BookId, command.Translation);
    }

    public static FormatAdded AddFormat(AddFormat command, Book state)
    {
        var format = command.Format;

        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot add format of a book that is not in the Editing state.");

        if (state.Formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        return new FormatAdded(state.BookId, format);
    }

    public static FormatRemoved RemoveFormat(RemoveFormat command, Book state)
    {
        var format = command.Format;

        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot remove format of a book that is not in the Editing state.");

        var existingFormat = state.Formats.FirstOrDefault(f => f.FormatType == format.FormatType);
        if (existingFormat == null)
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        return new FormatRemoved(state.BookId, format);
    }

    public static Approved Approve(Approve command, Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot approve a book that is not in the Editing state.");

        if (state.ReviewersCount < 3)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        return new Approved(state.BookId, command.CommitteeApproval);
    }

    public static MovedToPrinting MoveToPrinting(Print command, Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot move to Printing state from the current state.");

        if (!state.IsApproved)
            throw new InvalidOperationException("Cannot move to the Printing state until the book has been approved.");

        if (state.ReviewersCount < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");

        if (!state.PublishingHouse.IsGenreLimitReached(state.Genre))
            throw new InvalidOperationException("Cannot move to the Printing state until the genre limit is reached.");

        return new MovedToPrinting(state.BookId);
    }

    public static Published MoveToPublished(Publish command, Book state)
    {
        if (state.CurrentState != State.Printing || state.TranslationsCount < 5)
            throw new InvalidOperationException("Cannot move to Published state from the current state.");

        if (state.ReviewersCount < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

        return new Published(state.BookId, state.ISBN, state.Title, state.Author);
    }

    public static MovedToOutOfPrint MoveToOutOfPrint(MovedToOutOfPrint command, Book state)
    {
        if (state.CurrentState != State.Published)
            throw new InvalidOperationException("Cannot move to Out of Print state from the current state.");

        double totalCopies = state.Formats.Sum(f => f.TotalCopies);
        double totalSoldCopies = state.Formats.Sum(f => f.SoldCopies);
        if ((totalSoldCopies / totalCopies) > 0.1)
            throw new InvalidOperationException(
                "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

        return new MovedToOutOfPrint(state.BookId);
    }
}
