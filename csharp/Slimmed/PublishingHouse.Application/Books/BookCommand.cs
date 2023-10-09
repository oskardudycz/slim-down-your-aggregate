using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Entities;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Application.Books;

public abstract record BookCommand
{
    public record CreateDraft(
        BookId BookId,
        Title Title,
        AuthorIdOrData Author,
        PublisherId PublisherId,
        PositiveInt Edition,
        Genre? Genre
    ): BookCommand;

    public record AddChapter(BookId BookId, ChapterTitle Title, ChapterContent Content): BookCommand;

    public record MoveToEditing(BookId BookId): BookCommand;

    public record AddTranslation(BookId BookId, Translation Translation): BookCommand;

    public record AddFormat(BookId BookId, Format Format): BookCommand;

    public record RemoveFormat(BookId BookId, Format Format): BookCommand;

    public record AddReviewer(BookId BookId, Reviewer Reviewer): BookCommand;

    public record Approve(BookId BookId, CommitteeApproval CommitteeApproval): BookCommand;

    public record SetISBN(BookId BookId, ISBN ISBN): BookCommand;

    public record MoveToPrinting(BookId BookId): BookCommand;

    public record MoveToPublished(BookId BookId): BookCommand;

    public record MoveToOutOfPrint(BookId BookId): BookCommand;

    private BookCommand() { }
}
