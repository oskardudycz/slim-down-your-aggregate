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
            (null as IPublishingHouse)!, //TODO: Change that
            new Publisher(new PublisherId(book.Publisher.Id), new PublisherName(book.Publisher.Name)),
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

    public static BookEntity MapToEntity(this Book book, PublishingHouseDbContext dbContext) =>
        new BookEntity
        {
            Id = book.Id.Value,
            CurrentState = (BookEntity.State)book.CurrentState,
            Title = book.Title.Value,
            Author =
                dbContext.ChangeTracker.Entries().Select(a => a.Entity).OfType<AuthorEntity>()
                    .Single(a => a.Id == book.Author.Id.Value),
            Genre = book.Genre?.Value,
            Publisher =
                dbContext.ChangeTracker.Entries().Select(a => a.Entity).OfType<PublisherEntity>()
                    .Single(a => a.Id == book.Publisher.Id.Value),
            Edition = book.Edition.Value,
            ISBN = book.ISBN?.Value,
            PublicationDate = book.PublicationDate,
            TotalPages = book.TotalPages?.Value,
            NumberOfIllustrations = book.NumberOfIllustrations?.Value,
            BindingType = book.BindingType?.Value,
            Summary = book.Summary?.Value,
            Reviewers = book.Reviewers
                .Select(r =>
                    new ReviewerEntity { Id = r.Id.Value, Name = r.Name.Value }
                ).ToList(),
            Chapters = book.Chapters
                .Select(c =>
                    new ChapterEntity { Number = c.Number.Value, Title = c.Title.Value, Content = c.Content.Value }
                )
                .ToList(),
            Translations = book.Translations
                .Select(t =>
                    new ValueObjects.TranslationVO
                    {
                        Language =
                            new LanguageEntity { Id = t.Language.Id.Value, Name = t.Language.Name.Value },
                        Translator = new TranslatorEntity { Id = t.Translator.Id.Value, Name = t.Translator.Name.Value }
                    }
                )
                .ToList(),
            Formats = book.Formats
                .Select(f => new Entities.FormatEntity
                {
                    FormatType = f.FormatType.Value,
                    TotalCopies = f.TotalCopies.Value,
                    SoldCopies = f.SoldCopies.Value
                })
                .ToList(),
            CommitteeApproval = book.CommitteeApproval != null
                ? new ValueObjects.CommitteeApprovalVO(
                    book.CommitteeApproval.IsApproved,
                    book.CommitteeApproval.Feedback.Value
                )
                : null
        };

    public static void UpdateFrom(this BookEntity entity, Book book)
    {
        entity.CurrentState = (BookEntity.State)book.CurrentState;
        entity.Title = book.Title.Value;

        // entity.Author =
        //     new AuthorEntity
        //     {
        //         Id = book.Author.Id.Value,
        //         FirstName = book.Author.FirstName.Value,
        //         LastName = book.Author.LastName.Value
        //     };
        entity.Genre = book.Genre?.Value;
        // entity.Publisher =
        //     new PublisherEntity { Id = book.Publisher.Id.Value, Name = book.Publisher.Name.Value };
        entity.Edition = book.Edition.Value;
        entity.ISBN = book.ISBN?.Value;
        entity.PublicationDate = book.PublicationDate;
        entity.TotalPages = book.TotalPages?.Value;
        entity.NumberOfIllustrations = book.NumberOfIllustrations?.Value;
        entity.BindingType = book.BindingType?.Value;
        entity.Summary = book.Summary?.Value;
        // entity.Reviewers = book.Reviewers
        //     .Select(r =>
        //         new ReviewerEntity { Id = r.Id.Value, Name = r.Name.Value }
        //     ).ToList();
        entity.Chapters.Update(
            book.Chapters.ToList(),
            a => e => e.Number == a.Number.Value,
            (e, a) => e.Update(a),
            a => new ChapterEntity { Number = a.Number.Value, Title = a.Title.Value, Content = a.Content.Value }
        );
        // entity.Translations = book.Translations
        //     .Select(t =>
        //         new ValueObjects.Translation
        //         {
        //             Language =
        //                 new LanguageEntity { Id = t.Language.Id.Value, Name = t.Language.Name.Value },
        //             Translator = new TranslatorEntity { Id = t.Translator.Id.Value, Name = t.Translator.Name.Value }
        //         }
        //     )
        //     .ToList();
        // entity.Formats = book.Formats
        //     .Select(f => new Entities.Format
        //     {
        //         FormatType = f.FormatType.Value, TotalCopies = f.TotalCopies.Value, SoldCopies = f.SoldCopies.Value
        //     })
        //     .ToList();
        // entity.CommitteeApproval = book.CommitteeApproval != null
        //     ? new ValueObjects.CommitteeApproval(
        //         book.CommitteeApproval.IsApproved,
        //         book.CommitteeApproval.Feedback.Value
        //     )
        //     : null;
    }
}
