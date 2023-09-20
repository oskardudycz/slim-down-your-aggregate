using PublishingHouse.Books.Entities;
using PublishingHouse.Core;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books;

public abstract record BookEvent: IDomainEvent
{
    public record DraftCreated(
        BookId BookId,
        Title Title,
        Author Author,
        Publisher Publisher,
        PositiveInt Edition,
        Genre? Genre
    ): BookEvent;

    public record ChapterAdded(
        BookId BookId,
        Chapter Chapter
    ): BookEvent;

    public record FormatAdded(
        BookId BookId,
        Format Format
    ): BookEvent;

    public record FormatRemoved(
        BookId BookId,
        Format Format
    ): BookEvent;

    public record TranslationAdded(
        BookId BookId,
        Translation Translation
    ): BookEvent;

    public record TranslationRemoved(
        BookId BookId,
        Translation Translation
    ): BookEvent;

    public record ReviewerAdded(
        BookId BookId,
        Reviewer Reviewer
    ): BookEvent;

    public record MovedToEditing(
        BookId BookId
    ): BookEvent;

    public record Approved(
        BookId BookId,
        CommitteeApproval CommitteeApproval
    ): BookEvent;

    public record ISBNSet(
        BookId BookId,
        ISBN ISBN
    ): BookEvent;

    public record MovedToPrinting
    (
        BookId BookId
    ): BookEvent;

    public record Published(
        BookId BookId,
        ISBN ISBN,
        Title Title,
        Author Author
    ): BookEvent;

    public record MovedToOutOfPrint
    (
        BookId BookId
    ): BookEvent;

    private BookEvent() { }
}
