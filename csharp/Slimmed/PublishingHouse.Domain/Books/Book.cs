using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.Validation;
using PublishingHouse.Core.ValueObjects;
using static PublishingHouse.Books.BookEvent;

namespace PublishingHouse.Books;

public abstract class Book
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public BookId Id { get; private set; }

    public class Initial: Book
    {
        public Initial(BookId bookId): base(bookId) { }

        public DraftCreated CreateDraft(
            Title title,
            Author author,
            Publisher publisher,
            PositiveInt edition,
            Genre? genre
        ) =>
            new DraftCreated(Id, title, author, publisher, edition, genre);
    }

    public class Draft: Book
    {
        private readonly Genre? genre;
        private readonly List<ChapterTitle> chapterTitles;
        private int ChaptersCount => chapterTitles.Count;

        internal Draft(
            BookId bookId,
            Genre? genre,
            List<ChapterTitle> chapterTitles
        ): base(bookId)
        {
            this.genre = genre;
            this.chapterTitles = chapterTitles;
        }

        public ChapterAdded AddChapter(ChapterTitle title, ChapterContent content)
        {
            if (!title.Value.StartsWith("Chapter " + (chapterTitles.Count + 1)))
                throw new InvalidOperationException(
                    $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {chapterTitles.Count + 1}'");

            if (chapterTitles.Contains(title))
                throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

            var chapter = new Chapter(new ChapterNumber(chapterTitles.Count + 1), title, content);
            chapterTitles.Add(title);

            return new ChapterAdded(Id, chapter);
        }

        public MovedToEditing MoveToEditing()
        {
            if (ChaptersCount < 1)
                throw new InvalidOperationException(
                    "A book must have at least one chapter to move to the Editing state.");

            if (genre == null)
                throw new InvalidOperationException("Book can be moved to the editing only when genre is specified");

            return new MovedToEditing(Id, genre);
        }
    }

    public class UnderEditing: Book
    {
        private readonly Genre? genre;
        private bool isISBNSet;
        private bool isApproved;
        private readonly List<ReviewerId> reviewers;
        private readonly List<LanguageId> translationLanguages;
        private readonly List<FormatType> formatTypes;

        internal UnderEditing(
            BookId bookId,
            Genre? genre,
            bool isISBNSet,
            bool isApproved,
            List<ReviewerId> reviewers,
            List<LanguageId> translationLanguages,
            List<FormatType> formatTypes
        ): base(bookId)
        {
            this.genre = genre;
            this.isISBNSet = isISBNSet;
            this.isApproved = isApproved;
            this.reviewers = reviewers;
            this.translationLanguages = translationLanguages;
            this.formatTypes = formatTypes;
        }

        public TranslationAdded AddTranslation(Translation translation, PositiveInt maximumNumberOfTranslations)
        {
            var languageId = translation.Language.Id;

            if (translationLanguages.Contains(languageId))
                throw new InvalidOperationException($"Translation to {translation.Language.Name} already exists.");

            if (translationLanguages.Count >= maximumNumberOfTranslations.Value)
                throw new InvalidOperationException(
                    $"Cannot add more translations. Maximum {maximumNumberOfTranslations.Value} translations are allowed.");

            translationLanguages.Add(languageId);

            return new TranslationAdded(Id, translation);
        }

        public FormatAdded AddFormat(Format format)
        {
            if (formatTypes.Contains(format.FormatType))
                throw new InvalidOperationException($"Format {format.FormatType} already exists.");

            formatTypes.Add(format.FormatType);

            return new FormatAdded(Id, format);
        }

        public FormatRemoved RemoveFormat(Format format)
        {
            if (!formatTypes.Remove(format.FormatType))
                throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

            return new FormatRemoved(Id, format);
        }

        public ReviewerAdded AddReviewer(Reviewer reviewer)
        {
            if (reviewers.Contains(reviewer.Id))
                throw new InvalidOperationException(
                    $"{reviewer.Name} is already a reviewer.");

            reviewers.Add(reviewer.Id);

            return new ReviewerAdded(Id, reviewer);
        }

        public Approved Approve(CommitteeApproval committeeApproval, PositiveInt minimumReviewersRequiredForApproval)
        {
            if (reviewers.Count < minimumReviewersRequiredForApproval.Value)
                throw new InvalidOperationException(
                    "A book cannot be approved unless it has been reviewed by at least three reviewers.");

            isApproved = true;

            return new Approved(Id, committeeApproval);
        }

        public ISBNSet SetISBN(ISBN isbn)
        {
            if (isISBNSet)
                throw new InvalidOperationException(
                    "Cannot change already set ISBN.");

            isISBNSet = true;

            return new ISBNSet(Id, isbn);
        }

        public MovedToPrinting MoveToPrinting(IPublishingHouse publishingHouse)
        {
            if (isApproved)
                throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

            if (genre == null)
                throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

            if (!publishingHouse.IsGenreLimitReached(genre))
                throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

            return new MovedToPrinting(Id);
        }
    }

    public class InPrint: Book
    {
        internal InPrint(BookId bookId): base(bookId)
        {
        }

        public Published MoveToPublished() =>
            new Published(Id);
    }

    public class PublishedBook: Book
    {
        private readonly PositiveInt totalCopies;
        private readonly PositiveInt totalSoldCopies;

        private Ratio UnsoldCopiesRatio =>
            new((totalCopies.Value - totalSoldCopies.Value) / (decimal)totalCopies.Value);

        public PublishedBook(
            BookId bookId,
            PositiveInt totalCopies,
            PositiveInt totalSoldCopies
        ): base(bookId)
        {
            this.totalCopies = totalCopies;
            this.totalSoldCopies = totalSoldCopies;
        }

        public MovedToOutOfPrint MoveToOutOfPrint(Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint)
        {
            if (UnsoldCopiesRatio.CompareTo(maxAllowedUnsoldCopiesRatioToGoOutOfPrint) > 0)
                throw new InvalidOperationException(
                    "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

            return new MovedToOutOfPrint(Id);
        }
    }

    public class OutOfPrint: Book
    {
        public OutOfPrint(BookId bookId): base(bookId)
        {
        }
    }

    public static Book Evolve(Book book, BookEvent @event)
    {
        switch (@event)
        {
            case DraftCreated draftCreated:
            {
                return book is Initial
                    ? new Draft(draftCreated.BookId, draftCreated.Genre, new List<ChapterTitle>())
                    : book;
            }
            case MovedToEditing movedToEditing:
            {
                return book is Draft
                    ? new UnderEditing(movedToEditing.BookId, movedToEditing.Genre, false, false,
                        new List<ReviewerId>(), new List<LanguageId>(), new List<FormatType>())
                    : book;
            }
            case MovedToPrinting movedToPrinting:
            {
                return book is Draft
                    // TODO: Add methods to set total items per format
                    ? new InPrint(movedToPrinting.BookId)
                    : book;
            }
            case Published movedToPrinting:
            {
                return book is Draft
                    // TODO: Add methods to set sold copies
                    ? new PublishedBook(movedToPrinting.BookId, new PositiveInt(1), new PositiveInt(1))
                    : book;
            }
            case MovedToOutOfPrint movedToOutOfPrint:
            {
                return book is Draft
                    // TODO: Add methods to set sold copies
                    ? new OutOfPrint(movedToOutOfPrint.BookId)
                    : book;
            }
            default:
                return book;
        }
    }

    private Book(BookId bookId) => Id = bookId;

    public class Factory: IBookFactory
    {
        public Book Create(
            BookId bookId,
            State state,
            Title title,
            Author author,
            Genre? genre,
            ISBN? isbn,
            CommitteeApproval? committeeApproval,
            List<Reviewer> reviewers,
            List<Chapter> chapters,
            List<Translation> translations,
            List<Format> formats
        ) =>
            state switch
            {
                State.Writing =>
                    new Draft(bookId, genre, chapters.Select(ch => ch.Title).ToList()),
                State.Editing =>
                    new UnderEditing(
                        bookId,
                        genre,
                        isbn != null,
                        committeeApproval != null,
                        reviewers.Select(r => r.Id).ToList(),
                        translations.Select(t => t.Language.Id).ToList(),
                        formats.Select(f => f.FormatType).ToList()
                    ),
                State.Printing =>
                    new InPrint(bookId),
                State.Published =>
                    new PublishedBook(
                        bookId,
                        new PositiveInt(formats.Sum(f => f.TotalCopies.Value)),
                        new PositiveInt(formats.Sum(f => f.SoldCopies.Value))
                    ),
                State.OutOfPrint =>
                    new OutOfPrint(bookId),
                _ => throw new InvalidOperationException()
            };
    }
}
