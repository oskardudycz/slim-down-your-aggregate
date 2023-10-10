using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.UnderEditing;

using static BookEvent.UnderEditingEvent;
using static BookEvent.InPrintEvent;

public record BookUnderEditing: Book
{
    private readonly Genre? genre;
    private readonly bool isISBNSet;
    private readonly bool isApproved;
    private readonly List<ReviewerId> reviewers;
    private readonly List<LanguageId> translationLanguages;
    private readonly List<FormatType> formatTypes;

    internal BookUnderEditing(
        BookId bookId,
        Genre? genre,
        bool isISBNSet,
        bool isApproved,
        List<ReviewerId> reviewers,
        List<LanguageId> translationLanguages,
        List<FormatType> formatTypes
    ): base(bookId)
    {
        this.genre = genre;
        this.isISBNSet = isISBNSet;
        this.isApproved = isApproved;
        this.reviewers = reviewers;
        this.translationLanguages = translationLanguages;
        this.formatTypes = formatTypes;
    }

    public TranslationAdded AddTranslation(Translation translation, PositiveInt maximumNumberOfTranslations)
    {
        var languageId = translation.Language.Id;

        if (translationLanguages.Contains(languageId))
            throw new InvalidOperationException($"Translation to {translation.Language.Name} already exists.");

        if (translationLanguages.Count >= maximumNumberOfTranslations.Value)
            throw new InvalidOperationException(
                $"Cannot add more translations. Maximum {maximumNumberOfTranslations.Value} translations are allowed.");

        return new TranslationAdded(Id, translation);
    }

    public FormatAdded AddFormat(Format format)
    {
        if (formatTypes.Contains(format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        return new FormatAdded(Id, format);
    }

    public FormatRemoved RemoveFormat(Format format)
    {
        if (!formatTypes.Remove(format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        return new FormatRemoved(Id, format);
    }

    public ReviewerAdded AddReviewer(Reviewer reviewer)
    {
        if (reviewers.Contains(reviewer.Id))
            throw new InvalidOperationException(
                $"{reviewer.Name} is already a reviewer.");

        return new ReviewerAdded(Id, reviewer);
    }

    public Approved Approve(CommitteeApproval committeeApproval, PositiveInt minimumReviewersRequiredForApproval)
    {
        if (reviewers.Count < minimumReviewersRequiredForApproval.Value)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        return new Approved(Id, committeeApproval);
    }

    public ISBNSet SetISBN(ISBN isbn)
    {
        if (isISBNSet)
            throw new InvalidOperationException(
                "Cannot change already set ISBN.");

        return new ISBNSet(Id, isbn);
    }

    public MovedToPrinting MoveToPrinting(IPublishingHouse publishingHouse)
    {
        if (isApproved)
            throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

        if (genre == null)
            throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

        if (!publishingHouse.IsGenreLimitReached(genre))
            throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

        return new MovedToPrinting(Id);
    }

    public static BookUnderEditing Evolve(BookUnderEditing book, BookEvent.UnderEditingEvent @event) =>
        @event switch
        {
            MovedToEditing movedToEditing =>
                new BookUnderEditing(
                    movedToEditing.BookId,
                    movedToEditing.Genre,
                    false,
                    false,
                    new List<ReviewerId>(),
                    new List<LanguageId>(),
                    new List<FormatType>()
                ),

            TranslationAdded(_, var translation) =>
                new BookUnderEditing(
                    book.Id,
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages.Union(new[] { translation.Language.Id }).ToList(),
                    book.formatTypes
                ),

            TranslationRemoved(_, var translation) =>
                new BookUnderEditing(
                    book.Id,
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages.Where(t => t != translation.Language.Id).ToList(),
                    book.formatTypes
                ),

            FormatAdded(_, var format) =>
                new BookUnderEditing(
                    book.Id,
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages,
                    book.formatTypes.Union(new[] { format.FormatType }).ToList()
                ),

            FormatRemoved(_, var format) =>
                new BookUnderEditing(
                    book.Id,
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages,
                    book.formatTypes.Where(t => t != format.FormatType).ToList()
                ),

            ReviewerAdded(_, var reviewer) =>
                new BookUnderEditing(
                    book.Id,
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers.Union(new[] { reviewer.Id }).ToList(),
                    book.translationLanguages,
                    book.formatTypes
                ),

            Approved =>
                new BookUnderEditing(
                    book.Id,
                    book.genre,
                    book.isISBNSet,
                    true,
                    book.reviewers,
                    book.translationLanguages,
                    book.formatTypes
                ),

            ISBNSet =>
                new BookUnderEditing(
                    book.Id,
                    book.genre,
                    true,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages,
                    book.formatTypes
                ),

            _ => book
        };
}
