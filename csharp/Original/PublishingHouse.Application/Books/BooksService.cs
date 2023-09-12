using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Books;
using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Publishers;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Books.Services;

namespace PublishingHouse.Application.Books;

public class BooksService: IBooksService
{
    public async Task CreateDraft(CreateDraftCommand command, CancellationToken ct)
    {
        var (bookId, title, authorIdOrData, publisherId, positiveInt, genre) = command;

        var book = Book.CreateDraft(
            bookId,
            title,
            await authorProvider.GetOrCreate(authorIdOrData, ct),
            (null as IPublishingHouse)!, //TODO: Consider making it smarter
            await publisherProvider.GetById(publisherId, ct),
            positiveInt,
            genre
        );

        await repository.Add(book, ct);
    }

    public async Task AddChapter(AddChapterCommand command, CancellationToken ct)
    {
        var (bookId, chapterTitle, chapterContent) = command;

        var book = await repository.FindById(bookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        book.AddChapter(chapterTitle, chapterContent);

        await repository.Update(book, ct);
    }

    public async Task MoveToEditing(MoveToEditingCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        book.MoveToEditing();

        await repository.Update(book, ct);
    }

    public BooksService(
        IBooksRepository repository,
        IAuthorProvider authorProvider,
        IPublisherProvider publisherProvider
    )
    {
        this.repository = repository;
        this.authorProvider = authorProvider;
        this.publisherProvider = publisherProvider;
    }

    private readonly IBooksRepository repository;
    private readonly IAuthorProvider authorProvider;
    private readonly IPublisherProvider publisherProvider;
}
