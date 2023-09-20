using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.Aggregates;
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
        private readonly Genre? genre;
        private readonly List<Chapter> chapters;

        internal Draft(
            BookId bookId,
            Genre? genre,
            List<Chapter>? chapters = null
        ): base(bookId)
        {
            this.genre = genre;
            this.chapters = chapters ?? new List<Chapter>();
        }

        public void AddChapter(ChapterTitle title, ChapterContent content)
        {
            if (chapters.Any(chap => chap.Title.Value == title.Value))
                throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

            if (chapters.Count > 0 && !chapters.Last().Title.Value.StartsWith("Chapter " + (chapters.Count)))
                throw new InvalidOperationException(
                    $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {chapters.Count + 1}'");

            var chapter = new Chapter(new ChapterNumber(chapters.Count + 1), title, content);
            chapters.Add(chapter);

            AddDomainEvent(new ChapterAdded(Id, chapter));
        }

        public void MoveToEditing()
        {
            if (chapters.Count < 1)
                throw new InvalidOperationException(
                    "A book must have at least one chapter to move to the Editing state.");

            if (genre == null)
                throw new InvalidOperationException("Book can be moved to the editing only when genre is specified");

            AddDomainEvent(new MovedToEditing(Id));
        }
    }

    public class UnderEditing: Book
    {
        private readonly Genre? genre;
        private ISBN? isbn;
        private CommitteeApproval? committeeApproval;
        private readonly List<Reviewer> reviewers;
        private readonly List<Chapter> chapters;
        private readonly List<Translation> translations;
        private readonly List<Format> formats;

        internal UnderEditing(
            BookId bookId,
            Genre? genre,
            ISBN? isbn = null,
            CommitteeApproval? committeeApproval = null,
            List<Reviewer>? reviewers = null,
            List<Chapter>? chapters = null,
            List<Translation>? translations = null,
            List<Format>? formats = null
        ): base(bookId)
        {
            this.genre = genre;
            this.isbn = isbn;
            this.committeeApproval = committeeApproval;
            this.reviewers = reviewers ?? new List<Reviewer>();
            this.chapters = chapters ?? new List<Chapter>();
            this.translations = translations ?? new List<Translation>();
            this.formats = formats ?? new List<Format>();
        }

        public void AddTranslation(Translation translation)
        {
            if (translations.Count >= 5)
                throw new InvalidOperationException(
                    "Cannot add more translations. Maximum 5 translations are allowed.");

            translations.Add(translation);

            AddDomainEvent(new TranslationAdded(Id, translation));
        }

        public void AddFormat(Format format)
        {
            if (formats.Any(f => f.FormatType == format.FormatType))
                throw new InvalidOperationException($"Format {format.FormatType} already exists.");

            formats.Add(format);

            AddDomainEvent(new FormatAdded(Id, format));
        }

        public void RemoveFormat(Format format)
        {
            var existingFormat = formats.FirstOrDefault(f => f.FormatType == format.FormatType);
            if (existingFormat == null)
                throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

            formats.Remove(existingFormat);

            AddDomainEvent(new FormatRemoved(Id, format));
        }

        public void AddReviewer(Reviewer reviewer)
        {
            if (reviewers.Contains(reviewer))
                throw new InvalidOperationException(
                    $"{reviewer.Name} is already a reviewer.");

            reviewers.Add(reviewer);

            AddDomainEvent(new ReviewerAdded(Id, reviewer));
        }

        public void Approve(CommitteeApproval committeeApproval)
        {
            if (reviewers.Count < 3)
                throw new InvalidOperationException(
                    "A book cannot be approved unless it has been reviewed by at least three reviewers.");

            this.committeeApproval = committeeApproval;

            AddDomainEvent(new Approved(Id, committeeApproval));
        }

        public void SetISBN(ISBN isbn)
        {
            if (this.isbn != null)
                throw new InvalidOperationException(
                    "Cannot change already set ISBN.");

            this.isbn = isbn;

            AddDomainEvent(new ISBNSet(Id, isbn));
        }

        public void MoveToPrinting(IPublishingHouse publishingHouse)
        {
            if (chapters.Count < 1)
                throw new InvalidOperationException(
                    "A book must have at least one chapter to move to the printing state.");

            if (committeeApproval == null)
                throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

            if (reviewers.Count < 3)
                throw new InvalidOperationException(
                    "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");

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
        private ISBN? isbn;
        private readonly List<Reviewer> reviewers;

        internal InPrint(
            BookId bookId,
            Title title,
            Author author,
            ISBN? isbn = null,
            List<Reviewer>? reviewers = null
        ): base(bookId)
        {
            this.title = title;
            this.author = author;
            this.isbn = isbn;
            this.reviewers = reviewers ?? new List<Reviewer>();
        }

        public void MoveToPublished()
        {
            if (isbn == null)
                throw new InvalidOperationException("Cannot move to Published state without ISBN.");

            if (reviewers.Count < 3)
                throw new InvalidOperationException(
                    "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

            AddDomainEvent(new Published(Id, isbn, title, author));
        }
    }

    public class PublishedBook: Book
    {
        private List<Format> formats;

        public PublishedBook(
            BookId bookId,
            List<Format> formats
        ): base(bookId)
        {
            this.formats = formats;
        }


        public void MoveToOutOfPrint()
        {
            double totalCopies = formats.Sum(f => f.TotalCopies.Value);
            double totalSoldCopies = formats.Sum(f => f.SoldCopies.Value);
            if ((totalSoldCopies / totalCopies) > 0.1)
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
                    new Draft(bookId, genre, chapters),
                State.Editing =>
                    new UnderEditing(
                        bookId,
                        genre,
                        isbn,
                        committeeApproval,
                        reviewers,
                        chapters,
                        translations,
                        formats
                        ),
                State.Printing =>
                    new InPrint(bookId, title, author, isbn, reviewers),
                State.Published =>
                    new PublishedBook(bookId, formats),
                State.OutOfPrint =>
                    new OutOfPrint(bookId),
                _ => throw new InvalidOperationException()
            };
    }
}
