using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed;

public abstract record BookEvent
{
    public record WritingStarted(
        BookId BookId,
        Genre Genre,
        Title Title,
        Author Author,
        ISBN ISBN
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

    public record MovedToEditing(
        BookId BookId
    ): BookEvent;

    public record Approved(
        BookId BookId,
        CommitteeApproval CommitteeApproval
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
