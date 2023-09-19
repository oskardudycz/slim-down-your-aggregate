using PublishingHouse.Application.Books.Commands;

namespace PublishingHouse.Application.Books;

public interface IBooksService
{
    Task CreateDraft(CreateDraftCommand command, CancellationToken ct);
    Task AddChapter(AddChapterCommand command, CancellationToken ct);
    Task MoveToEditing(MoveToEditingCommand command, CancellationToken ct);
}
