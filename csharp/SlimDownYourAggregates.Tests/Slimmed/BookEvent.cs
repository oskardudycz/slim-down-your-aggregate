using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed;

public abstract record BookEvent
{
    public record ChapterAdded(
        BookId BookId,
        Chapter Chapter
    ): BookEvent;

    public record Approved(
        BookId BookId,
        CommitteeApproval CommitteeApproval
    ): BookEvent;

    public record MovedToEditing(
        BookId BookId
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

    public record FormatAdded(
        BookId BookId,
        Format Translation
    ): BookEvent;

    public record FormatRemoved(
        BookId BookId,
        Format Translation
    ): BookEvent;

    public record TranslationAdded(
        BookId BookId,
        Translation Translation
    ): BookEvent;

    private BookEvent() { }
}
