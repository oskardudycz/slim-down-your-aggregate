using PublishingHouse.Books.Entities;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books.UnderEditing;

using static UnderEditingEvent;
using static InPrintEvent;

public record BookUnderEditing
(
    Genre? Genre,
    bool IsISBNSet,
    bool IsApproved,
    IReadOnlyList<ReviewerId> Reviewers,
    IReadOnlyList<LanguageId> TranslationLanguages,
    IReadOnlyList<(FormatType FormatType, PositiveInt TotalCopies)> Formats
): Book
{
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
                    book.Genre,
                    book.IsISBNSet,
                    book.IsApproved,
                    book.Reviewers,
                    book.TranslationLanguages.Union(new[] { translation.Language.Id }).ToList(),
                    book.Formats
                ),
            TranslationRemoved (var translation) =>
                new BookUnderEditing(
                    book.Genre,
                    book.IsISBNSet,
                    book.IsApproved,
                    book.Reviewers,
                    book.TranslationLanguages.Where(t => t != translation.Language.Id).ToList(),
                    book.Formats
                ),
            FormatAdded (var format) =>
                new BookUnderEditing(
                    book.Genre,
                    book.IsISBNSet,
                    book.IsApproved,
                    book.Reviewers,
                    book.TranslationLanguages,
                    book.Formats.Union(new[] { (format.FormatType, format.TotalCopies) }).ToList()
                ),
            FormatRemoved (var format) =>
                new BookUnderEditing(
                    book.Genre,
                    book.IsISBNSet,
                    book.IsApproved,
                    book.Reviewers,
                    book.TranslationLanguages,
                    book.Formats.Where(t => t.FormatType != format.FormatType).ToList()
                ),
            ReviewerAdded (var reviewer) =>
                new BookUnderEditing(
                    book.Genre,
                    book.IsISBNSet,
                    book.IsApproved,
                    book.Reviewers.Union(new[] { reviewer.Id }).ToList(),
                    book.TranslationLanguages,
                    book.Formats
                ),
            Approved =>
                new BookUnderEditing(
                    book.Genre,
                    book.IsISBNSet,
                    true,
                    book.Reviewers,
                    book.TranslationLanguages,
                    book.Formats
                ),
            ISBNSet =>
                new BookUnderEditing(
                    book.Genre,
                    true,
                    book.IsApproved,
                    book.Reviewers,
                    book.TranslationLanguages,
                    book.Formats
                ),
            _ => Initial
        };

    public static readonly BookUnderEditing Initial =
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
