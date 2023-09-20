using PublishingHouse.Books;
using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.Tracking;
using PublishingHouse.Core.ValueObjects;
using PublishingHouse.Persistence.Authors;
using PublishingHouse.Persistence.Books.Entities;
using PublishingHouse.Persistence.Languages;
using PublishingHouse.Persistence.Publishers;
using PublishingHouse.Persistence.Reviewers;
using PublishingHouse.Persistence.Translators;
using Format = PublishingHouse.Books.Entities.Format;

namespace PublishingHouse.Persistence.Books.Mappers;

public static class BookEntityMapper
{
    public static Book MapToAggregate(this BookEntity book, IBookFactory bookFactory) =>
        bookFactory.Create(
            new BookId(book.Id),
            (Book.State)book.CurrentState,
            new Title(book.Title),
            new Author(new AuthorId(book.Author.Id), new AuthorFirstName(book.Author.FirstName),
                new AuthorLastName(book.Author.LastName)),
            book.Genre != null ? new Genre(book.Genre) : null,
            book.ISBN != null ? new ISBN(book.ISBN) : null,
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
                        new Translator(new TranslatorId(t.Translator.Id),
                            new TranslatorName(t.Translator.Name))
                    )
                )
                .ToList(),
            book.Formats
                .Select(f => new Format(new FormatType(f.FormatType), new PositiveInt(f.TotalCopies),
                    new PositiveInt(f.SoldCopies)))
                .ToList()
        );
}
