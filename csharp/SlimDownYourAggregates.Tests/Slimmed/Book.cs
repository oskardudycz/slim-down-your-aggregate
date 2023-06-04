using SlimDownYourAggregates.Tests.Slimmed.Entities;
using SlimDownYourAggregates.Tests.Slimmed.Services;
using static SlimDownYourAggregates.Tests.Slimmed.BookEvent;

namespace SlimDownYourAggregates.Tests.Slimmed;

public record Book(
    BookId BookId,
    Title Title,
    Author Author,
    Genre Genre,
    List<Reviewer> Reviewers,
    IPublishingHouse PublishingHouse,
    ISBN ISBN,
    List<Chapter> Chapters,
    List<Translation> Translations,
    List<Format> Formats,
    Book.State CurrentState = Book.State.Writing,
    CommitteeApproval? CommitteeApproval = null
)
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public Book Evolve<T>(Book book, T @event) where T : BookEvent
    {
        return @event switch
        {
            ChapterAdded chapterAdded =>
                book with { Chapters = Chapters.Union(new[] { chapterAdded.Chapter }).ToList() },

            MovedToEditing ignore =>
                book with { CurrentState = State.Editing },

            FormatAdded formatAdded =>
                book with { Formats = Formats.Union(new[] { formatAdded.Format }).ToList() },

            FormatRemoved formatRemoved =>
                book with { Formats = Formats.Where(f => f.FormatType != formatRemoved.Format.FormatType).ToList() },

            TranslationAdded translationAdded =>
                book with { Translations = Translations.Union(new[] { translationAdded.Translation }).ToList() },

            Approved approved =>
                book with { CommitteeApproval = approved.CommitteeApproval },

            MovedToPrinting ignore =>
                book with { CurrentState = State.Printing },

            Published ignore =>
                book with { CurrentState = State.Published },

            MovedToOutOfPrint ignore =>
                book with { CurrentState = State.OutOfPrint },

            _ => book
        };
    }
}
