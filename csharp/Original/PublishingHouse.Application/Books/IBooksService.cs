using PublishingHouse.Application.Books.Commands;

namespace PublishingHouse.Application.Books;

public interface IBooksService
{
    void CreateDraft(CreateDraftCommand command);
}