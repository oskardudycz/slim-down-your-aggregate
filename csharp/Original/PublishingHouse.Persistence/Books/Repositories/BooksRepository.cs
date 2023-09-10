using PublishingHouse.Books;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.ValueObjects;
using PublishingHouse.Persistence.Languages;
using PublishingHouse.Persistence.Publishers;
using PublishingHouse.Persistence.Reviewers;
using PublishingHouse.Persistence.Translators;

namespace PublishingHouse.Persistence.Books.Repositories;

public class BooksRepository: IBooksRepository
{
    private readonly IBooksFactory booksFactory;

    public BooksRepository(IBooksFactory booksFactory) =>
        this.booksFactory = booksFactory;

    public async Task<Book?> FindById(BookId bookId)
    {
        var book = await PublishingHouseContext.Find(bookId.Value);

        if (book == null)
            return null;

        return booksFactory.Create(
            new BookId(book.Id),
            (Book.State)book.CurrentState,
            new Title(book.Title),
            new Author(new AuthorId(book.AuthorEntity.Id), new AuthorFirstName(book.AuthorEntity.FirstName),
                new AuthorLastName(book.AuthorEntity.LastName)),
            (null as IPublishingHouse)!, //TODO: Change that
            new Publisher(new PublisherId(book.PublisherEntity.Id), new PublisherName(book.PublisherEntity.Name)),
            new PositiveInt(book.Edition),
            book.Genre != null ? new Genre(book.Genre) : null,
            book.ISBN != null ? new ISBN(book.ISBN) : null,
            book.PublicationDate,
            book.TotalPages.HasValue ? new PositiveInt(book.TotalPages.Value) : null,
            book.NumberOfIllustrations.HasValue ? new PositiveInt(book.NumberOfIllustrations.Value) : null,
            book.BindingType != null ? new NonEmptyString(book.BindingType) : null,
            book.Summary != null ? new NonEmptyString(book.Summary) : null,
            book.CommitteeApproval != null
                ? new CommitteeApproval(book.CommitteeApproval.IsApproved,
                    new NonEmptyString(book.CommitteeApproval.Feedback))
                : null,
            book.Reviewers
                .Select(r => new Reviewer(new ReviewerId(r.Id), new ReviewerName(r.Name)))
                .ToList(),
            book.Chapters
                .Select(c =>
                    new Chapter(
                        new ChapterId(c.Id),
                        new ChapterNumber(c.Number),
                        new ChapterTitle(c.Title),
                        new ChapterContent(c.Content)
                    )
                )
                .ToList(),
            book.Translations
                .Select(t =>
                    new Translation(
                        new Language(new LanguageId(t.Language.Id), new LanguageName(t.Language.Name)),
                        new Translator(new TranslatorId(t.TranslatorEntity.Id), new TranslatorName(t.TranslatorEntity.Name))
                    )
                )
                .ToList(),
            book.Formats
                .Select(f => new Format(new FormatType(f.FormatType), new PositiveInt(f.TotalCopies),
                    new PositiveInt(f.SoldCopies)))
                .ToList()
        );
    }

    public Task Add(Book book) =>
        PublishingHouseContext.Add(
            new BookEntity
            {
                Id = book.Id,
                CurrentState = (BookEntity.State)book.CurrentState,
                Title = book.Title.Value,
                AuthorEntity =
                    new Authors.AuthorEntity(book.Author.Id.Value, book.Author.FirstName.Value, book.Author.LastName.Value),
                Genre = book.Genre?.Value,
                PublisherEntity = new PublisherEntity(book.Publisher.Id.Value, book.Publisher.Name.Value),
                Edition = book.Edition.Value,
                ISBN = book.ISBN?.Value,
                PublicationDate = book.PublicationDate,
                TotalPages = book.TotalPages?.Value,
                NumberOfIllustrations = book.NumberOfIllustrations?.Value,
                BindingType = book.BindingType?.Value,
                Summary = book.Summary?.Value,
                Reviewers = book.Reviewers
                    .Select(r =>
                        new ReviewerEntity(r.Id.Value, r.Name.Value)
                    ).ToList(),
                Chapters = book.Chapters
                    .Select(c =>
                        new Entities.ChapterEntity(
                            c.Id.Value,
                            c.Number.Value,
                            c.Title.Value,
                            c.Content.Value
                        )
                    )
                    .ToList(),
                Translations = book.Translations
                    .Select(t => new ValueObjects.Translation(
                        new LanguageEntity(t.Language.Id.Value, t.Language.Name.Value),
                        new TranslatorEntity(t.Language.Id.Value, t.Translator.Name.Value)))
                    .ToList(),
                Formats = book.Formats
                    .Select(f => new ValueObjects.Format(f.FormatType.Value, f.TotalCopies.Value, f.SoldCopies.Value))
                    .ToList(),
                CommitteeApproval = book.CommitteeApproval != null
                    ? new ValueObjects.CommitteeApproval(
                        book.CommitteeApproval.IsApproved,
                        book.CommitteeApproval.Feedback.Value
                    )
                    : null
            }
        );
}
