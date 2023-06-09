using SlimDownYourAggregates.Tests.Slimmed.Entities;
using static SlimDownYourAggregates.Tests.Slimmed.BookEvent;

namespace SlimDownYourAggregates.Tests.Slimmed;

public abstract record Book
{
    public record Initial: Book;

    public record InWriting(
        BookId BookId,
        Genre Genre,
        Title Title,
        Author Author,
        ISBN Isbn,
        List<String> ChapterTitles
    ): Book;

    public record InEditing(
        BookId BookId,
        Genre Genre,
        Title Title,
        Author Author,
        ISBN Isbn,
        List<Format> Formats,
        int TranslationsCount,
        int ReviewersCount,
        bool IsApproved
    ): Book;


    public record InPrinting(
        BookId BookId,
        Title Title,
        Author Author,
        ISBN ISBN,
        List<Format> Formats,
        int ReviewersCount,
        int TranslationsCount
    ): Book;

    public record InPublishing(
        BookId BookId,
        List<Format> Formats
    ): Book;

    public record OutOfPrint: Book;

    public static Book Evolve<T>(Book book, T @event) where T : BookEvent
    {
        return @event switch
        {
            WritingStarted writingStarted =>
                book is Initial
                    ? new InWriting(
                        writingStarted.BookId,
                        writingStarted.Genre,
                        writingStarted.Title,
                        writingStarted.Author,
                        writingStarted.ISBN,
                        new List<string>()
                    )
                    : book,

            ChapterAdded chapterAdded =>
                book is InWriting inWriting
                    ? inWriting with
                    {
                        ChapterTitles = inWriting.ChapterTitles.Union(new[] { chapterAdded.Chapter.Title.Value })
                            .ToList()
                    }
                    : book,

            MovedToEditing ignore =>
                book is InWriting inWriting
                    ? new InEditing(
                        inWriting.BookId,
                        inWriting.Genre,
                        inWriting.Title,
                        inWriting.Author,
                        inWriting.Isbn,
                        new List<Format>(),
                        0,
                        0,
                        false
                    )
                    : book,

            FormatAdded formatAdded =>
                book is InEditing inEditing
                    ? inEditing with { Formats = inEditing.Formats.Union(new[] { formatAdded.Format }).ToList() }
                    : book,

            FormatRemoved formatRemoved =>
                book is InEditing inEditing
                    ? inEditing with
                    {
                        Formats = inEditing.Formats.Where(f => f.FormatType != formatRemoved.Format.FormatType)
                            .ToList()
                    }
                    : book,

            TranslationAdded translationAdded =>
                book is InEditing inEditing
                    ? inEditing with { TranslationsCount = inEditing.TranslationsCount + 1 }
                    : book,

            Approved approved =>
                book is InEditing inEditing
                    ? inEditing with { IsApproved = true }
                    : book,

            MovedToPrinting ignore =>
                book is InEditing inEditing
                    ? new InPrinting(
                        inEditing.BookId,
                        inEditing.Title,
                        inEditing.Author,
                        inEditing.Isbn,
                        inEditing.Formats,
                        inEditing.ReviewersCount,
                        inEditing.TranslationsCount
                    )
                    : book,

            Published ignore =>
                book is InPrinting inEditing
                    ? new InPublishing(inEditing.BookId, inEditing.Formats)
                    : book,

            MovedToOutOfPrint ignore =>
                book is InPublishing inPublishing
                    ? new OutOfPrint()
                    : book,

            _ => book
        };
    }
}
