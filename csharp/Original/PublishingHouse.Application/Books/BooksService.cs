using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Authors;
using PublishingHouse.Books;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Books.Services;
using PublishingHouse.Publishers;

namespace PublishingHouse.Application.Books;

public class BooksService: IBooksService
{
    public async Task CreateDraft(CreateDraftCommand command)
    {
        var (bookId, title, authorIdOrData, publisherId, positiveInt, genre) = command;

        var book = Book.CreateDraft(
            bookId,
            title,
            await authorProvider.GetOrCreate(authorIdOrData),
            (null as IPublishingHouse)!, //TODO: Consider making it smarter
            await publisherProvider.GetById(publisherId),
            positiveInt,
            genre
        );

        await booksRepository.Add(book);
    }

    public BooksService(
        IBooksRepository booksRepository,
        IAuthorProvider authorProvider,
        IPublisherProvider publisherProvider
    )
    {
        this.booksRepository = booksRepository;
        this.authorProvider = authorProvider;
        this.publisherProvider = publisherProvider;
    }

    private readonly IBooksRepository booksRepository;
    private readonly IAuthorProvider authorProvider;
    private readonly IPublisherProvider publisherProvider;
}
