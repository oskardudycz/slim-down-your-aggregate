using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Application.Books.DTOs;
using PublishingHouse.Persistence;

namespace PublishingHouse.Application.Books;

public class BooksService: IBooksService
{
    public void CreateDraft(CreateDraftCommand command)
    {
        BooksContext.Books.Add(command.BookId, new BookDetails(command.BookId));
    }
}
