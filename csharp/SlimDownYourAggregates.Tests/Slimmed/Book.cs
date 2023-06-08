using SlimDownYourAggregates.Tests.Slimmed.Entities;
using SlimDownYourAggregates.Tests.Slimmed.Services;
using static SlimDownYourAggregates.Tests.Slimmed.BookEvent;

namespace SlimDownYourAggregates.Tests.Slimmed;

public record Book(
    BookId BookId,
    Title Title,
    Author Author,
    Genre Genre,
    int ReviewersCount,
    IPublishingHouse PublishingHouse,
    ISBN ISBN,
    List<string> ChapterTitles,
    int TranslationsCount,
    List<Format> Formats,
    bool IsApproved,
    Book.State CurrentState = Book.State.Writing
)
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public Book Evolve<T>(Book book, T @event) where T : BookEvent
    {
        return @event switch
        {
            ChapterAdded chapterAdded =>
                book with { ChapterTitles = ChapterTitles.Union(new[] { chapterAdded.Chapter.Title.Value }).ToList() },

            MovedToEditing ignore =>
                book with { CurrentState = State.Editing },

            FormatAdded formatAdded =>
                book with { Formats = Formats.Union(new[] { formatAdded.Format }).ToList() },

            FormatRemoved formatRemoved =>
                book with { Formats = Formats.Where(f => f.FormatType != formatRemoved.Format.FormatType).ToList() },

            TranslationAdded translationAdded =>
                book with { TranslationsCount = TranslationsCount + 1},

            Approved approved =>
                book with { IsApproved = true },

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
