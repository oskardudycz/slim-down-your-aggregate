using PublishingHouse.Application.Books.Commands;
using PublishingHouse.Books;
using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Publishers;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Books.Services;

namespace PublishingHouse.Application.Books;

public class BooksService: IBooksService
{
    public async Task CreateDraft(CreateDraftCommand command, CancellationToken ct)
    {
        var (bookId, title, author, publisherId, edition, genre) = command;
        var authorEntity = await authorProvider.GetOrCreate(author, ct);
        var publisherEntity = await publisherProvider.GetById(publisherId, ct);

        await Handle<Book.Initial>(
            bookId,
            book =>
                book.CreateDraft(
                    title,
                    authorEntity,
                    publisherEntity,
                    edition,
                    genre
                ), ct);
    }

    public Task AddChapter(AddChapterCommand command, CancellationToken ct) =>
        Handle<Book.Draft>(command.BookId, book =>
        {
            var (_, chapterTitle, chapterContent) = command;
            return book.AddChapter(chapterTitle, chapterContent);
        }, ct);

    public Task MoveToEditing(MoveToEditingCommand command, CancellationToken ct) =>
        Handle<Book.Draft>(command.BookId, book => book.MoveToEditing(), ct);

    public Task AddTranslation(AddTranslationCommand command, CancellationToken ct) =>
        Handle<Book.UnderEditing>(command.BookId, book => book.AddTranslation(command.Translation), ct);

    public Task AddFormat(AddFormatCommand command, CancellationToken ct) =>
        Handle<Book.UnderEditing>(command.BookId, book => book.AddFormat(command.Format), ct);

    public Task RemoveFormat(RemoveFormatCommand command, CancellationToken ct) =>
        Handle<Book.UnderEditing>(command.BookId, book => book.RemoveFormat(command.Format), ct);

    public Task AddReviewer(AddReviewerCommand command, CancellationToken ct) =>
        Handle<Book.UnderEditing>(command.BookId, book => book.AddReviewer(command.Reviewer), ct);

    public Task Approve(ApproveCommand command, CancellationToken ct) =>
        Handle<Book.UnderEditing>(command.BookId, book => book.Approve(command.CommitteeApproval), ct);

    public Task SetISBN(SetISBNCommand command, CancellationToken ct) =>
        Handle<Book.UnderEditing>(command.BookId, book => book.SetISBN(command.ISBN), ct);

    public Task MoveToPrinting(MoveToPrintingCommand command, CancellationToken ct) =>
        Handle<Book.UnderEditing>(command.BookId, book => book.MoveToPrinting((null as IPublishingHouse)!), ct);

    public Task MoveToPublished(MoveToPublishedCommand command, CancellationToken ct) =>
        Handle<Book.InPrint>(command.BookId, book => book.MoveToPublished(), ct);

    public Task MoveToOutOfPrint(MoveToOutOfPrintCommand command, CancellationToken ct) =>
        Handle<Book.PublishedBook>(command.BookId, book => book.MoveToOutOfPrint(), ct);

    private async Task Handle<T>(BookId id, Func<T, BookEvent> handle, CancellationToken ct) where T : Book
    {
        var book = await repository.FindById(id, ct) ?? GetDefault(id);

        if (book is not T typedBook) throw new InvalidOperationException();

        var @event = handle(typedBook);

        await repository.Store(id, new[] { @event }, ct);
    }

    private Book GetDefault(BookId bookId) => new Book.Initial(bookId);

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
