using PublishingHouse.Books;
using PublishingHouse.Books.Authors;
using PublishingHouse.Books.Draft;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.InPrint;
using PublishingHouse.Books.Published;
using PublishingHouse.Books.Publishers;
using PublishingHouse.Books.Services;
using PublishingHouse.Books.UnderEditing;
using PublishingHouse.Core.ValueObjects;
using PublishingHouse.Persistence.Books.Mappers;
using PublishingHouse.Persistence.Books.Repositories;

namespace PublishingHouse.Application.Books;

using static DraftCommand;
using static UnderEditingCommand;
using static InPrintCommand;
using static PublishedCommand;

public class BooksService: IBooksService
{
    public async Task CreateDraft(BookApplicationCommand.CreateDraftAndSetupAuthorAndPublisher command,
        CancellationToken ct)
    {
        var (bookId, title, author, publisherId, edition, genre) = command;
        var authorEntity = await authorProvider.GetOrCreate(author, ct);
        var publisherEntity = await publisherProvider.GetById(publisherId, ct);

        await Handle<InitialBook>(
            bookId,
            book =>
                DraftDecider.CreateDraft(
                    new CreateDraft(bookId, title, authorEntity, publisherEntity, edition, genre),
                    book
                ), ct);
    }

    public Task AddChapter(AddChapter command, CancellationToken ct) =>
        Handle<BookDraft>(command.BookId, book => DraftDecider.AddChapter(command, book), ct);

    public Task MoveToEditing(MoveToEditing command, CancellationToken ct) =>
        Handle<BookDraft>(command.BookId, book => DraftDecider.MoveToEditing(command, book), ct);

    public Task AddTranslation(BookApplicationCommand.AddTranslation applicationCommand, CancellationToken ct)
    {
        var command = new AddTranslation(
            applicationCommand.BookId,
            applicationCommand.Translation,
            maximumNumberOfTranslations
        );

        return Handle<BookUnderEditing>(applicationCommand.BookId,
            book => UnderEditingDecider.AddTranslation(command, book), ct);
    }

    public Task AddFormat(AddFormat command, CancellationToken ct) =>
        Handle<BookUnderEditing>(command.BookId, book => UnderEditingDecider.AddFormat(command, book), ct);

    public Task RemoveFormat(RemoveFormat command, CancellationToken ct) =>
        Handle<BookUnderEditing>(command.BookId, book => UnderEditingDecider.RemoveFormat(command, book), ct);

    public Task AddReviewer(AddReviewer command, CancellationToken ct) =>
        Handle<BookUnderEditing>(command.BookId, book => UnderEditingDecider.AddReviewer(command, book), ct);

    public Task Approve(BookApplicationCommand.Approve applicationCommand, CancellationToken ct)
    {
        var command = new Approve(
            applicationCommand.BookId,
            applicationCommand.CommitteeApproval,
            minimumReviewersRequiredForApproval
        );
        return Handle<BookUnderEditing>(command.BookId,
            book => UnderEditingDecider.Approve(command, book),
            ct);
    }

    public Task SetISBN(SetISBN command, CancellationToken ct) =>
        Handle<BookUnderEditing>(command.BookId, book => UnderEditingDecider.SetISBN(command, book), ct);

    public Task MoveToPrinting(MoveToPrinting command, CancellationToken ct) =>
        Handle<BookUnderEditing>(command.BookId,
            book =>
            {
                if (book.Genre != null && !publishingHouse.IsGenreLimitReached(book.Genre))
                    throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

                return UnderEditingDecider.MoveToPrinting(new MoveToPrinting(command.BookId), book);
            },
            ct);

    public Task MoveToPublished(MoveToPublished command, CancellationToken ct) =>
        Handle<BookInPrint>(command.BookId, book => InPrintDecider.MoveToPublished(command, book), ct);

    public Task MoveToOutOfPrint(BookApplicationCommand.MoveToOutOfPrint applicationCommand, CancellationToken ct)
    {
        var command = new MoveToOutOfPrint(applicationCommand.BookId, maxAllowedUnsoldCopiesRatioToGoOutOfPrint);

        return Handle<PublishedBook>(command.BookId,
            book => PublishedDecider.MoveToOutOfPrint(command, book), ct);
    }

    private Task Handle<T>(BookId id, Func<T, BookEvent> handle, CancellationToken ct) where T : Book =>
        repository.GetAndUpdate(id, (entity) =>
        {
            var aggregate = entity?.MapToAggregate(bookFactory) ?? GetDefault();

            if (aggregate is not T typedBook) throw new InvalidOperationException();

            var @event = handle(typedBook);

            return new[] { @event };
        }, ct);

    private Book GetDefault() => InitialBook.Initial;

    public BooksService(
        IBooksRepository repository,
        IBookFactory bookFactory,
        IAuthorProvider authorProvider,
        IPublisherProvider publisherProvider,
        IPublishingHouse publishingHouse,
        PositiveInt minimumReviewersRequiredForApproval,
        PositiveInt maximumNumberOfTranslations,
        Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint
    )
    {
        this.repository = repository;
        this.bookFactory = bookFactory;
        this.authorProvider = authorProvider;
        this.publisherProvider = publisherProvider;
        this.publishingHouse = publishingHouse;
        this.minimumReviewersRequiredForApproval = minimumReviewersRequiredForApproval;
        this.maximumNumberOfTranslations = maximumNumberOfTranslations;
        this.maxAllowedUnsoldCopiesRatioToGoOutOfPrint = maxAllowedUnsoldCopiesRatioToGoOutOfPrint;
    }

    private readonly IBooksRepository repository;
    private readonly IBookFactory bookFactory;
    private readonly IAuthorProvider authorProvider;
    private readonly IPublisherProvider publisherProvider;
    private readonly IPublishingHouse publishingHouse;
    private readonly PositiveInt minimumReviewersRequiredForApproval;
    private readonly PositiveInt maximumNumberOfTranslations;
    private readonly Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint;
}
