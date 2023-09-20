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
        var (bookId, title, author, publisherId, edition, genre) = command;

        var book = new Book.Initial(bookId);

        book.CreateDraft(
            title,
            await authorProvider.GetOrCreate(author, ct),
            await publisherProvider.GetById(publisherId, ct),
            edition,
            genre
        );

        await repository.Add(book, ct);
    }

    public async Task AddChapter(AddChapterCommand command, CancellationToken ct)
    {
        var (bookId, chapterTitle, chapterContent) = command;

        var book = await repository.FindById(bookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.Draft draft) throw new InvalidOperationException();

        draft.AddChapter(chapterTitle, chapterContent);

        await repository.Update(book, ct);
    }

    public async Task MoveToEditing(MoveToEditingCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.Draft draft) throw new InvalidOperationException();

        draft.MoveToEditing();

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
