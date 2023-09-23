using PublishingHouse.Application.Books.Commands;

namespace PublishingHouse.Application.Books;

public interface IBooksService
{
    Task CreateDraft(CreateDraftCommand command, CancellationToken ct);
    Task AddChapter(AddChapterCommand command, CancellationToken ct);
    Task MoveToEditing(MoveToEditingCommand command, CancellationToken ct);

    Task AddTranslation(AddTranslationCommand command, CancellationToken ct);
    Task AddFormat(AddFormatCommand command, CancellationToken ct);
    Task RemoveFormat(RemoveFormatCommand command, CancellationToken ct);
    Task AddReviewer(AddReviewerCommand command, CancellationToken ct);
    Task Approve(ApproveCommand command, CancellationToken ct);
    Task SetISBN(SetISBNCommand command, CancellationToken ct);
    Task MoveToPublished(MoveToPublishedCommand command, CancellationToken ct);
    Task MoveToPrinting(MoveToPrintingCommand command, CancellationToken ct);
    Task MoveToOutOfPrint(MoveToOutOfPrintCommand command, CancellationToken ct);
}
