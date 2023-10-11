using PublishingHouse.Books.Entities;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.UnderEditing;

using static UnderEditingEvent;
using static InPrintEvent;

public static class UnderEditingDecider
{
    public static TranslationAdded AddTranslation(
        BookUnderEditing state,
        Translation translation,
        PositiveInt maximumNumberOfTranslations
    )
    {
        var languageId = translation.Language.Id;

        if (state.TranslationLanguages.Contains(languageId))
            throw new InvalidOperationException($"Translation to {translation.Language.Name} already exists.");

        if (state.TranslationLanguages.Count >= maximumNumberOfTranslations.Value)
            throw new InvalidOperationException(
                $"Cannot add more translations. Maximum {maximumNumberOfTranslations.Value} translations are allowed.");

        return new TranslationAdded(translation);
    }

    public static FormatAdded AddFormat(BookUnderEditing state, Format format)
    {
        if (state.Formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        return new FormatAdded(format);
    }

    public static FormatRemoved RemoveFormat(BookUnderEditing state, Format format)
    {
        if (state.Formats.All(f => f.FormatType != format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        return new FormatRemoved(format);
    }

    public static ReviewerAdded AddReviewer(BookUnderEditing state, Reviewer reviewer)
    {
        if (state.Reviewers.Contains(reviewer.Id))
            throw new InvalidOperationException(
                $"{reviewer.Name} is already a reviewer.");

        return new ReviewerAdded(reviewer);
    }

    public static Approved Approve(
        BookUnderEditing state,
        CommitteeApproval committeeApproval,
        PositiveInt minimumReviewersRequiredForApproval
    )
    {
        if (state.Reviewers.Count < minimumReviewersRequiredForApproval.Value)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        return new Approved(committeeApproval);
    }

    public static ISBNSet SetISBN(BookUnderEditing state, ISBN isbn)
    {
        if (state.IsISBNSet)
            throw new InvalidOperationException(
                "Cannot change already set ISBN.");

        return new ISBNSet(isbn);
    }

    public static MovedToPrinting MoveToPrinting(BookUnderEditing state, IPublishingHouse publishingHouse)
    {
        if (state.IsApproved)
            throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

        if (state.Genre == null)
            throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

        if (!publishingHouse.IsGenreLimitReached(state.Genre))
            throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

        return new MovedToPrinting(new PositiveInt(state.Formats.Sum(f => f.TotalCopies.Value)));
    }
}
