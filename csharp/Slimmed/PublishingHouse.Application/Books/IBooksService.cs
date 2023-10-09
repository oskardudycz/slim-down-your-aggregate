namespace PublishingHouse.Application.Books;

using static BookCommand;

public interface IBooksService
{
    Task CreateDraft(CreateDraft command, CancellationToken ct);
    Task AddChapter(AddChapter command, CancellationToken ct);
    Task MoveToEditing(MoveToEditing command, CancellationToken ct);

    Task AddTranslation(AddTranslation command, CancellationToken ct);
    Task AddFormat(AddFormat command, CancellationToken ct);
    Task RemoveFormat(RemoveFormat command, CancellationToken ct);
    Task AddReviewer(AddReviewer command, CancellationToken ct);
    Task Approve(Approve command, CancellationToken ct);
    Task SetISBN(SetISBN command, CancellationToken ct);
    Task MoveToPublished(MoveToPublished command, CancellationToken ct);
    Task MoveToPrinting(MoveToPrinting command, CancellationToken ct);
    Task MoveToOutOfPrint(MoveToOutOfPrint command, CancellationToken ct);
}
