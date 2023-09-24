using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.Aggregates;
using PublishingHouse.Core.Validation;
using PublishingHouse.Core.ValueObjects;
using static PublishingHouse.Books.BookEvent;

namespace PublishingHouse.Books;

public abstract class Book: Aggregate<BookId, BookEvent>
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public class Initial: Book
    {
        public Initial(BookId bookId): base(bookId) { }

        public void CreateDraft(
            Title title,
            Author author,
            Publisher publisher,
            PositiveInt edition,
            Genre? genre
        ) =>
            AddDomainEvent(new DraftCreated(Id, title, author, publisher, edition, genre));
    }

    public class Draft: Book
    {
        private readonly bool isGenreSet;
        private readonly List<ChapterTitle> chapterTitles;
        private int ChaptersCount => chapterTitles.Count;

        internal Draft(
            BookId bookId,
            bool isGenreSet,
            List<ChapterTitle> chapterTitles
        ): base(bookId)
        {
            this.isGenreSet = isGenreSet;
            this.chapterTitles = chapterTitles;
        }

        public void AddChapter(ChapterTitle title, ChapterContent content)
        {
            if (!title.Value.StartsWith("Chapter " + (chapterTitles.Count + 1)))
                throw new InvalidOperationException(
                    $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {chapterTitles.Count + 1}'");

            if (chapterTitles.Contains(title))
                throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

            var chapter = new Chapter(new ChapterNumber(chapterTitles.Count + 1), title, content);
            chapterTitles.Add(title);

            AddDomainEvent(new ChapterAdded(Id, chapter));
        }

        public void MoveToEditing()
        {
            if (ChaptersCount < 1)
                throw new InvalidOperationException(
                    "A book must have at least one chapter to move to the Editing state.");

            if (!isGenreSet)
                throw new InvalidOperationException("Book can be moved to the editing only when genre is specified");

            AddDomainEvent(new MovedToEditing(Id));
        }
    }

    public class UnderEditing: Book
    {
        private readonly Genre? genre;
        private bool isISBNSet;
        private bool isApproved;
        private readonly PositiveInt chaptersCount;
        private readonly List<ReviewerId> reviewers;
        private readonly PositiveInt minimumReviewersRequiredForApproval;
        private readonly List<LanguageId> translationLanguages;
        private readonly PositiveInt maximumNumberOfTranslations;
        private readonly List<FormatType> formatTypes;

        internal UnderEditing(
            BookId bookId,
            Genre? genre,
            bool isISBNSet,
            bool isApproved,
            PositiveInt chaptersCount,
            List<ReviewerId> reviewers,
            PositiveInt minimumReviewersRequiredForApproval,
            List<LanguageId> translationLanguages,
            PositiveInt maximumNumberOfTranslations,
            List<FormatType> formatTypes
        ): base(bookId)
        {
            this.genre = genre;
            this.isISBNSet = isISBNSet;
            this.isApproved = isApproved;
            this.chaptersCount = chaptersCount;
            this.reviewers = reviewers;
            this.minimumReviewersRequiredForApproval = minimumReviewersRequiredForApproval;
            this.translationLanguages = translationLanguages;
            this.maximumNumberOfTranslations = maximumNumberOfTranslations;
            this.formatTypes = formatTypes;
        }

        public void AddTranslation(Translation translation)
        {
            var languageId = translation.Language.Id;

            if (translationLanguages.Contains(languageId))
                throw new InvalidOperationException($"Translation to {translation.Language.Name} already exists.");

            if (translationLanguages.Count >= maximumNumberOfTranslations.Value)
                throw new InvalidOperationException(
                    $"Cannot add more translations. Maximum {maximumNumberOfTranslations.Value} translations are allowed.");

            translationLanguages.Add(languageId);

            AddDomainEvent(new TranslationAdded(Id, translation));
        }

        public void AddFormat(Format format)
        {
            if (formatTypes.Contains(format.FormatType))
                throw new InvalidOperationException($"Format {format.FormatType} already exists.");

            formatTypes.Add(format.FormatType);

            AddDomainEvent(new FormatAdded(Id, format));
        }

        public void RemoveFormat(Format format)
        {
            if (!formatTypes.Remove(format.FormatType))
                throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

            AddDomainEvent(new FormatRemoved(Id, format));
        }

        public void AddReviewer(Reviewer reviewer)
        {
            if (reviewers.Contains(reviewer.Id))
                throw new InvalidOperationException(
                    $"{reviewer.Name} is already a reviewer.");

            reviewers.Add(reviewer.Id);

            AddDomainEvent(new ReviewerAdded(Id, reviewer));
        }

        public void Approve(CommitteeApproval committeeApproval)
        {
            if (reviewers.Count < minimumReviewersRequiredForApproval.Value)
                throw new InvalidOperationException(
                    "A book cannot be approved unless it has been reviewed by at least three reviewers.");

            isApproved = true;

            AddDomainEvent(new Approved(Id, committeeApproval));
        }

        public void SetISBN(ISBN isbn)
        {
            if (isISBNSet)
                throw new InvalidOperationException(
                    "Cannot change already set ISBN.");

            isISBNSet = true;

            AddDomainEvent(new ISBNSet(Id, isbn));
        }

        public void MoveToPrinting(IPublishingHouse publishingHouse)
        {
            if (chaptersCount.Value < 1)
                throw new InvalidOperationException(
                    "A book must have at least one chapter to move to the printing state.");

            if (isApproved)
                throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

            if (genre == null)
                throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

            if (!publishingHouse.IsGenreLimitReached(genre))
                throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

            AddDomainEvent(new MovedToPrinting(Id));
        }
    }

    public class InPrint: Book
    {
        private readonly Title title;
        private readonly Author author;
        private readonly ISBN isbn;

        internal InPrint(
            BookId bookId,
            Title title,
            Author author,
            ISBN isbn
        ): base(bookId)
        {
            this.title = title;
            this.author = author;
            this.isbn = isbn;
        }

        public void MoveToPublished()
        {
            AddDomainEvent(new Published(Id, isbn, title, author));
        }
    }

    public class PublishedBook: Book
    {
        private readonly PositiveInt totalCopies;
        private readonly PositiveInt totalSoldCopies;
        private readonly Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint;

        private Ratio UnsoldCopiesRatio =>
            new((totalCopies.Value - totalSoldCopies.Value) / (decimal)totalCopies.Value);

        public PublishedBook(
            BookId bookId,
            PositiveInt totalCopies,
            PositiveInt totalSoldCopies,
            Ratio maxAllowedUnsoldCopiesRatioToGoOutOfPrint
        ): base(bookId)
        {
            this.totalCopies = totalCopies;
            this.totalSoldCopies = totalSoldCopies;
            this.maxAllowedUnsoldCopiesRatioToGoOutOfPrint = maxAllowedUnsoldCopiesRatioToGoOutOfPrint;
        }

        public void MoveToOutOfPrint()
        {
            if (UnsoldCopiesRatio.CompareTo(maxAllowedUnsoldCopiesRatioToGoOutOfPrint) > 0)
                throw new InvalidOperationException(
                    "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

            AddDomainEvent(new MovedToOutOfPrint(Id));
        }
    }

    public class OutOfPrint: Book
    {
        public OutOfPrint(BookId bookId): base(bookId)
        {
        }
    }

    private Book(BookId bookId): base(bookId)
    {
    }

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
                    new Draft(bookId, genre != null, chapters.Select(ch => ch.Title).ToList()),
                State.Editing =>
                    new UnderEditing(
                        bookId,
                        genre,
                        isbn != null,
                        committeeApproval != null,
                        new PositiveInt(chapters.Count),
                        reviewers.Select(r => r.Id).ToList(),
                        new PositiveInt(3), // this could be read from config, environment variables or database
                        translations.Select(t => t.Language.Id).ToList(),
                        new PositiveInt(5), // this could be read from config, environment variables or database
                        formats.Select(f => f.FormatType).ToList()
                    ),
                State.Printing =>
                    new InPrint(bookId, title, author, isbn.AssertNotNull()),
                State.Published =>
                    new PublishedBook(
                        bookId,
                        new PositiveInt(formats.Sum(f => f.TotalCopies.Value)),
                        new PositiveInt(formats.Sum(f => f.SoldCopies.Value)),
                        new Ratio(0.10m) // this could be read from config, environment variables or database
                    ),
                State.OutOfPrint =>
                    new OutOfPrint(bookId),
                _ => throw new InvalidOperationException()
            };
    }
}
