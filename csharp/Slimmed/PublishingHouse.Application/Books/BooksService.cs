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

        await repository.Store(book, ct);
    }

    public async Task AddChapter(AddChapterCommand command, CancellationToken ct)
    {
        var (bookId, chapterTitle, chapterContent) = command;

        var book = await repository.FindById(bookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.Draft draft) throw new InvalidOperationException();

        draft.AddChapter(chapterTitle, chapterContent);

        await repository.Store(book, ct);
    }

    public async Task MoveToEditing(MoveToEditingCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.Draft draft) throw new InvalidOperationException();

        draft.MoveToEditing();

        await repository.Store(book, ct);
    }

    public async Task AddTranslation(AddTranslationCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.UnderEditing underEditing) throw new InvalidOperationException();

        underEditing.AddTranslation(command.Translation);

        await repository.Store(book, ct);
    }

    public async Task AddFormat(AddFormatCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.UnderEditing underEditing) throw new InvalidOperationException();

        underEditing.AddFormat(command.Format);

        await repository.Store(book, ct);
    }

    public async Task RemoveFormat(RemoveFormatCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.UnderEditing underEditing) throw new InvalidOperationException();

        underEditing.RemoveFormat(command.Format);

        await repository.Store(book, ct);
    }

    public async Task AddReviewer(AddReviewerCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.UnderEditing underEditing) throw new InvalidOperationException();

        underEditing.AddReviewer(command.Reviewer);

        await repository.Store(book, ct);
    }

    public async Task Approve(ApproveCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.UnderEditing underEditing) throw new InvalidOperationException();

        underEditing.Approve(command.CommitteeApproval);

        await repository.Store(book, ct);
    }

    public async Task SetISBN(SetISBNCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.UnderEditing underEditing) throw new InvalidOperationException();

        underEditing.SetISBN(command.ISBN);

        await repository.Store(book, ct);
    }

    public async Task MoveToPrinting(MoveToPrintingCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.UnderEditing underEditing) throw new InvalidOperationException();

        underEditing.MoveToPrinting((null as IPublishingHouse)!);

        await repository.Store(book, ct);
    }

    public async Task MoveToPublished(MoveToPublishedCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.InPrint inPrint) throw new InvalidOperationException();

        inPrint.MoveToPublished();

        await repository.Store(book, ct);
    }

    public async Task MoveToOutOfPrint(MoveToOutOfPrintCommand command, CancellationToken ct)
    {
        var book = await repository.FindById(command.BookId, ct) ??
                   throw new InvalidOperationException(); // TODO: Add Explicit Not Found exception

        if (book is not Book.PublishedBook published) throw new InvalidOperationException();

        published.MoveToOutOfPrint();

        await repository.Store(book, ct);
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
