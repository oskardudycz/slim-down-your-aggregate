using PublishingHouse.Books.Entities;
using PublishingHouse.Core.Events;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Books;

public abstract record BookEvent: IDomainEvent
{
    public abstract record DraftEvent: BookEvent
    {
        public record DraftCreated(
            BookId BookId,
            Title Title,
            Author Author,
            Publisher Publisher,
            PositiveInt Edition,
            Genre? Genre
        ): DraftEvent;

        public record ChapterAdded(
            BookId BookId,
            Chapter Chapter
        ): DraftEvent;
    }

    public abstract record UnderEditingEvent: BookEvent
    {
        public record MovedToEditing(
            BookId BookId,
            Genre Genre
        ): UnderEditingEvent;

        public record TranslationAdded(
            BookId BookId,
            Translation Translation
        ): UnderEditingEvent;

        public record TranslationRemoved(
            BookId BookId,
            Translation Translation
        ): UnderEditingEvent;

        public record FormatAdded(
            BookId BookId,
            Format Format
        ): UnderEditingEvent;

        public record FormatRemoved(
            BookId BookId,
            Format Format
        ): UnderEditingEvent;

        public record ReviewerAdded(
            BookId BookId,
            Reviewer Reviewer
        ): UnderEditingEvent;

        public record Approved(
            BookId BookId,
            CommitteeApproval CommitteeApproval
        ): UnderEditingEvent;

        public record ISBNSet(
            BookId BookId,
            ISBN ISBN
        ): UnderEditingEvent;
    }

    public abstract record InPrintEvent: BookEvent
    {
        public record MovedToPrinting
        (
            BookId BookId
        ): InPrintEvent;
    }

    public abstract record PublishedEvent: BookEvent
    {
        public record Published(
            BookId BookId
        ): PublishedEvent;
    }

    public abstract record OutOfPrintEvent: BookEvent
    {
        public record MovedToOutOfPrint
        (
            BookId BookId
        ):OutOfPrintEvent;
    }

    private BookEvent() { }
}

public abstract record BookExternalEvent: IDomainEvent
{
    public record Published(
        BookId BookId,
        ISBN ISBN,
        Title Title,
        AuthorId AuthorId
    ): BookExternalEvent;

    private BookExternalEvent() { }
}
