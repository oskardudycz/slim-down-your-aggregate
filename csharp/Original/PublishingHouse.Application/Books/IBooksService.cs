using PublishingHouse.Application.Books.Commands;

namespace PublishingHouse.Application.Books;

public interface IBooksService
{
    Task CreateDraft(CreateDraftCommand command);
}
