using SlimDownYourAggregates.Tests.Slimmed.Core;
using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed;

public abstract record BookEvent: IDomainEvent
{
    public record ChapterAddedEvent(
        BookId BookId,
        Chapter Chapter
    ): BookEvent;

    public record BookApprovedEvent(
        BookId BookId,
        CommitteeApproval CommitteeApproval
    ): BookEvent;

    public record BookMovedToEditingEvent(
        BookId BookId
    ): BookEvent;

    public record BookMovedToPrintingEvent
    (
        BookId BookId
    ): BookEvent;

    public record BookPublishedEvent(
        BookId BookId,
        ISBN ISBN,
        Title Title,
        Author Author
    ): BookEvent;

    public record BookMovedToOutOfPrintEvent
    (
        BookId BookId
    ): BookEvent;

    public record FormatAddedEvent(
        BookId BookId,
        Format Translation
    ): BookEvent;

    public record FormatRemovedEvent(
        BookId BookId,
        Format Translation
    ): BookEvent;

    public record TranslationAddedEvent(
        BookId BookId,
        Translation Translation
    ): BookEvent;

    private BookEvent() { }
}
