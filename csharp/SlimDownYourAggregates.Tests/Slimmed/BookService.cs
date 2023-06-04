using SlimDownYourAggregates.Tests.Slimmed.Entities;
using static SlimDownYourAggregates.Tests.Slimmed.BookEvent;
using static SlimDownYourAggregates.Tests.Slimmed.Book;

namespace SlimDownYourAggregates.Tests.Slimmed;

public static class BookService
{
    public static ChapterAdded AddChapter(ChapterTitle title, ChapterContent content, Book state)
    {
        if (state.Chapters.Any(chap => chap.Title.Value == title.Value))
            throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

        if (state.Chapters.Count > 0 && state.Chapters.Last().Title.Value != "Chapter " + (state.Chapters.Count))
            throw new InvalidOperationException(
                $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {state.Chapters.Count + 1}'");

        var chapter = new Chapter(title, content);

        return new ChapterAdded(state.BookId, chapter);
    }

    public static MovedToEditing MoveToEditing(Book state)
    {
        if (state.CurrentState != State.Writing)
            throw new InvalidOperationException("Cannot move to Editing state from the current state.");

        if (state.Chapters.Count < 1)
            throw new InvalidOperationException("A book must have at least one chapter to move to the Editing state.");

        return new MovedToEditing(state.BookId);
    }

    public static TranslationAdded AddTranslation(Translation translation, Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot add translation of a book that is not in the Editing state.");

        if (state.Translations.Count >= 5)
            throw new InvalidOperationException("Cannot add more translations. Maximum 5 translations are allowed.");

        return new TranslationAdded(state.BookId, translation);
    }

    public static FormatAdded AddFormat(Format format, Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot add format of a book that is not in the Editing state.");

        if (state.Formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        return new FormatAdded(state.BookId, format);
    }

    public static FormatRemoved RemoveFormat(Format format, Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot remove format of a book that is not in the Editing state.");

        var existingFormat = state.Formats.FirstOrDefault(f => f.FormatType == format.FormatType);
        if (existingFormat == null)
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        return new FormatRemoved(state.BookId, format);
    }

    public static Approved Approve(CommitteeApproval committeeApproval, Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot approve a book that is not in the Editing state.");

        if (state.Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        return new Approved(state.BookId, committeeApproval);
    }

    public static MovedToPrinting MoveToPrinting(Book state)
    {
        if (state.CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot move to Printing state from the current state.");

        if (state.CommitteeApproval == null)
            throw new InvalidOperationException("Cannot move to the Printing state until the book has been approved.");

        if (state.Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");

        if (!state.PublishingHouse.IsGenreLimitReached(state.Genre))
            throw new InvalidOperationException("Cannot move to the Printing state until the genre limit is reached.");

        return new MovedToPrinting(state.BookId);
    }

    public static Published MoveToPublished(Book state)
    {
        if (state.CurrentState != State.Printing || state.Translations.Count < 5)
            throw new InvalidOperationException("Cannot move to Published state from the current state.");

        if (state.Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

        return new Published(state.BookId, state.ISBN, state.Title, state.Author);
    }

    public static MovedToOutOfPrint MoveToOutOfPrint(Book state)
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
