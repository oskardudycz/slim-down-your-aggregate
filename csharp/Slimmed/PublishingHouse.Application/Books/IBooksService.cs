using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Draft;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.Published;
using PublishingHouse.Books.UnderEditing;
using PublishingHouse.Core.ValueObjects;

namespace PublishingHouse.Application.Books;


using static DraftCommand;
using static UnderEditingCommand;
using static InPrintCommand;
using static PublishedCommand;

public interface IBooksService
{
    Task CreateDraft(CreateDraftAndSetupAuthorAndPublisher command, CancellationToken ct);
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

public record CreateDraftAndSetupAuthorAndPublisher(
    BookId BookId,
    Title Title,
    AuthorIdOrData Author,
    PublisherId PublisherId,
    PositiveInt Edition,
    Genre? Genre
);
