using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Books;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Books.Services;

namespace PublishingHouse.Application.Books;

public class BooksService: IBooksService
{
    public Task CreateDraft(CreateDraftCommand command)
    {
        var book = Book.CreateDraft(
            command.BookId,
            command.Title,
            command.Author,
            command.Genre,
            (null as IPublishingHouse)!, //TODO: Consider making it smarter
            command.Publisher,
            command.Edition
        );

        return booksRepository.Add(book);
    }

    private readonly IBooksRepository booksRepository;

    public BooksService(IBooksRepository booksRepository) =>
        this.booksRepository = booksRepository;
}
