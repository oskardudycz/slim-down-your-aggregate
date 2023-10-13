using PublishingHouse.Books.Entities;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.UnderEditing;

using static UnderEditingEvent;
using static UnderEditingCommand;
using static InPrintEvent;

public abstract record UnderEditingCommand: BookCommand
{
    public record AddTranslation(BookId BookId, Translation Translation, PositiveInt MaximumNumberOfTranslations): UnderEditingCommand;

    public record AddFormat(BookId BookId, Format Format): UnderEditingCommand;

    public record RemoveFormat(BookId BookId, Format Format): UnderEditingCommand;

    public record AddReviewer(BookId BookId, Reviewer Reviewer): UnderEditingCommand;

    public record Approve(BookId BookId, CommitteeApproval CommitteeApproval, PositiveInt MinimumReviewersRequiredForApproval): UnderEditingCommand;

    public record SetISBN(BookId BookId, ISBN ISBN): UnderEditingCommand;

    public record MoveToPrinting(BookId BookId): UnderEditingCommand;
}

public static class UnderEditingDecider
{
    public static TranslationAdded AddTranslation(AddTranslation command, BookUnderEditing state)
    {
        var (_, translation, maximumNumberOfTranslations) = command;
        var languageId = translation.Language.Id;

        if (state.TranslationLanguages.Contains(languageId))
            throw new InvalidOperationException($"Translation to {translation.Language.Name} already exists.");

        if (state.TranslationLanguages.Count >= maximumNumberOfTranslations.Value)
            throw new InvalidOperationException(
                $"Cannot add more translations. Maximum {maximumNumberOfTranslations.Value} translations are allowed.");

        return new TranslationAdded(translation);
    }

    public static FormatAdded AddFormat(AddFormat command, BookUnderEditing state)
    {
        var (_, format) = command;

        if (state.Formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        return new FormatAdded(format);
    }

    public static FormatRemoved RemoveFormat(RemoveFormat command, BookUnderEditing state)
    {
        var (_, format) = command;

        if (state.Formats.All(f => f.FormatType != format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        return new FormatRemoved(format);
    }

    public static ReviewerAdded AddReviewer(AddReviewer command, BookUnderEditing state)
    {
        var (_, reviewer) = command;

        if (state.Reviewers.Contains(reviewer.Id))
            throw new InvalidOperationException(
                $"{reviewer.Name} is already a reviewer.");

        return new ReviewerAdded(reviewer);
    }

    public static Approved Approve(Approve command, BookUnderEditing state)
    {
        var (_, committeeApproval, minimumReviewersRequiredForApproval) = command;

        if (state.Reviewers.Count < minimumReviewersRequiredForApproval.Value)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        return new Approved(committeeApproval);
    }

    public static ISBNSet SetISBN(SetISBN command, BookUnderEditing state)
    {
        var (_, isbn) = command;

        if (state.IsISBNSet)
            throw new InvalidOperationException(
                "Cannot change already set ISBN.");

        return new ISBNSet(isbn);
    }

    public static MovedToPrinting MoveToPrinting(MoveToPrinting command, BookUnderEditing state)
    {
        if (state.IsApproved)
            throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

        if (state.Genre == null)
            throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

        return new MovedToPrinting(new PositiveInt(state.Formats.Sum(f => f.TotalCopies.Value)));
    }
}
