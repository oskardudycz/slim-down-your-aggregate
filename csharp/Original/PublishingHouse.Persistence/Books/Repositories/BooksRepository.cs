using PublishingHouse.Books;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Repositories;
using PublishingHouse.Books.Services;

namespace PublishingHouse.Persistence.Books.Repositories;

public class BooksRepository: IBooksRepository
{
    private readonly IBooksFactory booksFactory;

    public BooksRepository(IBooksFactory booksFactory)
    {
        this.booksFactory = booksFactory;
    }

    public async Task<Book?> FindById(BookId bookId)
    {
        var book = await PublishingHouseContext.Find(bookId.Value);

        if (book == null)
            return null;

        return booksFactory.Create(
            new BookId(book.Id),
            (Book.State)book.CurrentState,
            new Title(book.Title),
            new Author(book.Author.FirstName, book.Author.LastName),
            book.Genre != null ? new Genre(book.Genre) : null,
            (null as IPublishingHouse)!, //TODO: Change that
            new Publisher(book.Publisher),
            book.Edition,
            book.ISBN != null ? new ISBN(book.ISBN) : null,
            book.PublicationDate,
            book.TotalPages,
            book.NumberOfIllustrations,
            book.BindingType,
            book.Summary,
            book.CommitteeApproval != null
                ? new CommitteeApproval(book.CommitteeApproval.IsApproved, book.CommitteeApproval.Feedback)
                : null,
            book.Reviewers
                .Select(r => new Reviewer(r.Name))
                .ToList(),
            book.Chapters
                .Select(c => new Chapter(new ChapterTitle(c.Title.Value), new ChapterContent(c.Content.Value)))
                .ToList(),
            book.Translations
                .Select(t => new Translation(new Language(t.Language.Name), new Translator(t.Translator.Name)))
                .ToList(),
            book.Formats
                .Select(f => new Format(f.FormatType, f.TotalCopies, f.SoldCopies))
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
                Author = new Entities.Author(book.Author.FirstName, book.Author.LastName),
                Genre = book.Genre?.Name,
                Publisher = book.Publisher.Name,
                Edition = book.Edition,
                ISBN = book.ISBN?.Number,
                PublicationDate = book.PublicationDate,
                TotalPages = book.TotalPages,
                NumberOfIllustrations = book.NumberOfIllustrations,
                BindingType = book.BindingType,
                Summary = book.Summary,
                Reviewers = book.Reviewers.Select(r => new Entities.Reviewer(r.Name)).ToList(),
                Chapters = book.Chapters.Select(c => new Entities.Chapter(new Entities.ChapterTitle(c.Title.Value),
                        new Entities.ChapterContent(c.Content.Value)))
                    .ToList(),
                Translations = book.Translations
                    .Select(t => new Entities.Translation(new Entities.Language(t.Language.Name),
                        new Entities.Translator(t.Translator.Name)))
                    .ToList(),
                Formats =
                    book.Formats.Select(f => new Entities.Format(f.FormatType, f.TotalCopies, f.SoldCopies)).ToList(),
                CommitteeApproval = book.CommitteeApproval != null
                    ? new Entities.CommitteeApproval(book.CommitteeApproval.IsApproved, book.CommitteeApproval.Feedback)
                    : null
            }
        );
}
