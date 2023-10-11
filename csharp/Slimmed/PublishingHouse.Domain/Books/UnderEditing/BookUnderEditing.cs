using PublishingHouse.Books.Entities;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.UnderEditing;

using static UnderEditingEvent;
using static InPrintEvent;

public record BookUnderEditing: Book
{
    private readonly Genre? genre;
    private readonly bool isISBNSet;
    private readonly bool isApproved;
    private readonly List<ReviewerId> reviewers;
    private readonly List<LanguageId> translationLanguages;
    private readonly List<(FormatType FormatType, PositiveInt TotalCopies)> formats;

    internal BookUnderEditing(
        Genre? genre,
        bool isISBNSet,
        bool isApproved,
        List<ReviewerId> reviewers,
        List<LanguageId> translationLanguages,
        List<(FormatType FormatType, PositiveInt TotalCopies)> formats
    )
    {
        this.genre = genre;
        this.isISBNSet = isISBNSet;
        this.isApproved = isApproved;
        this.reviewers = reviewers;
        this.translationLanguages = translationLanguages;
        this.formats = formats;
    }

    public TranslationAdded AddTranslation(Translation translation, PositiveInt maximumNumberOfTranslations)
    {
        var languageId = translation.Language.Id;

        if (translationLanguages.Contains(languageId))
            throw new InvalidOperationException($"Translation to {translation.Language.Name} already exists.");

        if (translationLanguages.Count >= maximumNumberOfTranslations.Value)
            throw new InvalidOperationException(
                $"Cannot add more translations. Maximum {maximumNumberOfTranslations.Value} translations are allowed.");

        return new TranslationAdded(translation);
    }

    public FormatAdded AddFormat(Format format)
    {
        if (formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        return new FormatAdded(format);
    }

    public FormatRemoved RemoveFormat(Format format)
    {
        if (formats.All(f => f.FormatType != format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        return new FormatRemoved(format);
    }

    public ReviewerAdded AddReviewer(Reviewer reviewer)
    {
        if (reviewers.Contains(reviewer.Id))
            throw new InvalidOperationException(
                $"{reviewer.Name} is already a reviewer.");

        return new ReviewerAdded(reviewer);
    }

    public Approved Approve(CommitteeApproval committeeApproval, PositiveInt minimumReviewersRequiredForApproval)
    {
        if (reviewers.Count < minimumReviewersRequiredForApproval.Value)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        return new Approved(committeeApproval);
    }

    public ISBNSet SetISBN(ISBN isbn)
    {
        if (isISBNSet)
            throw new InvalidOperationException(
                "Cannot change already set ISBN.");

        return new ISBNSet(isbn);
    }

    public MovedToPrinting MoveToPrinting(IPublishingHouse publishingHouse)
    {
        if (isApproved)
            throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

        if (genre == null)
            throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

        if (!publishingHouse.IsGenreLimitReached(genre))
            throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

        return new MovedToPrinting(new PositiveInt(formats.Sum(f=> f.TotalCopies.Value)));
    }

    public static BookUnderEditing Evolve(BookUnderEditing book, UnderEditingEvent @event) =>
        @event switch
        {
            MovedToEditing movedToEditing =>
                new BookUnderEditing(
                    movedToEditing.Genre,
                    false,
                    false,
                    new List<ReviewerId>(),
                    new List<LanguageId>(),
                    new List<(FormatType FormatType, PositiveInt TotalCopies)>()
                ),

            TranslationAdded(var translation) =>
                new BookUnderEditing(
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages.Union(new[] { translation.Language.Id }).ToList(),
                    book.formats
                ),
            TranslationRemoved (var translation) =>
                new BookUnderEditing(
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages.Where(t => t != translation.Language.Id).ToList(),
                    book.formats
                ),
            FormatAdded (var format) =>
                new BookUnderEditing(
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages,
                    book.formats.Union(new[] { (format.FormatType, format.TotalCopies) }).ToList()
                ),
            FormatRemoved (var format) =>
                new BookUnderEditing(
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages,
                    book.formats.Where(t => t.FormatType != format.FormatType).ToList()
                ),
            ReviewerAdded (var reviewer) =>
                new BookUnderEditing(
                    book.genre,
                    book.isISBNSet,
                    book.isApproved,
                    book.reviewers.Union(new[] { reviewer.Id }).ToList(),
                    book.translationLanguages,
                    book.formats
                ),
            Approved =>
                new BookUnderEditing(
                    book.genre,
                    book.isISBNSet,
                    true,
                    book.reviewers,
                    book.translationLanguages,
                    book.formats
                ),
            ISBNSet =>
                new BookUnderEditing(
                    book.genre,
                    true,
                    book.isApproved,
                    book.reviewers,
                    book.translationLanguages,
                    book.formats
                ),
            _ => Default
        };

    public static readonly BookUnderEditing Default =
        new BookUnderEditing(
            null,
            false,
            false,
            new List<ReviewerId>(),
            new List<LanguageId>(),
            new List<(FormatType FormatType, PositiveInt TotalCopies)>()
        );
}

public abstract record UnderEditingEvent: BookEvent
{
    public record MovedToEditing(Genre Genre): UnderEditingEvent;

    public record TranslationAdded(Translation Translation): UnderEditingEvent;

    public record TranslationRemoved(Translation Translation): UnderEditingEvent;

    public record FormatAdded(Format Format): UnderEditingEvent;

    public record FormatRemoved(Format Format): UnderEditingEvent;

    public record ReviewerAdded(Reviewer Reviewer): UnderEditingEvent;

    public record Approved(CommitteeApproval CommitteeApproval): UnderEditingEvent;

    public record ISBNSet(ISBN ISBN): UnderEditingEvent;
}
