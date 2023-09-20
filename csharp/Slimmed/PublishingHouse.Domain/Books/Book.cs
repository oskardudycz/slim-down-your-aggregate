using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Factories;
using PublishingHouse.Books.Services;
using PublishingHouse.Core.Aggregates;
using PublishingHouse.Core.ValueObjects;
using static PublishingHouse.Books.BookEvent;

namespace PublishingHouse.Books;

public class Book: Aggregate<BookId, BookEvent>
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public State CurrentState { get; private set; }
    public Title Title { get; }
    public Author Author { get; }

    public Genre? Genre { get; }
    public ISBN? ISBN { get; private set; }
    public CommitteeApproval? CommitteeApproval { get; private set; }

    private readonly List<Reviewer> reviewers;
    private readonly List<Chapter> chapters;
    private readonly List<Translation> translations;
    private readonly List<Format> formats;

    public static Book CreateDraft(
        BookId bookId,
        Title title,
        Author author,
        Publisher publisher,
        PositiveInt edition,
        Genre? genre
    )
    {
        var book = new Book(bookId, State.Writing, title, author, genre);
        book.AddDomainEvent(new DraftCreated(bookId, title, author, publisher, edition, genre));
        return book;
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
        if (CurrentState != State.Writing)
            throw new InvalidOperationException("Cannot move to Editing state from the current state.");

        if (chapters.Count < 1)
            throw new InvalidOperationException("A book must have at least one chapter to move to the Editing state.");

        if (Genre == null)
            throw new InvalidOperationException("Book can be moved to the editing only when genre is specified");

        CurrentState = State.Editing;

        AddDomainEvent(new MovedToEditing(Id));
    }

    public void AddTranslation(Translation translation)
    {
        if (CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot add translation of a book that is not in the Editing state.");

        if (translations.Count >= 5)
            throw new InvalidOperationException("Cannot add more translations. Maximum 5 translations are allowed.");

        translations.Add(translation);

        AddDomainEvent(new TranslationAdded(Id, translation));
    }

    public void AddFormat(Format format)
    {
        if (CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot add format of a book that is not in the Editing state.");

        if (formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        formats.Add(format);

        AddDomainEvent(new FormatAdded(Id, format));
    }

    public void RemoveFormat(Format format)
    {
        if (CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot remove format of a book that is not in the Editing state.");

        var existingFormat = formats.FirstOrDefault(f => f.FormatType == format.FormatType);
        if (existingFormat == null)
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        formats.Remove(existingFormat);

        AddDomainEvent(new FormatRemoved(Id, format));
    }

    public void AddReviewer(Reviewer reviewer)
    {
        if (CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot approve a book that is not in the Editing state.");

        if (reviewers.Contains(reviewer))
            throw new InvalidOperationException(
                $"{reviewer.Name} is already a reviewer.");

        reviewers.Add(reviewer);

        AddDomainEvent(new ReviewerAdded(Id, reviewer));
    }

    public void Approve(CommitteeApproval committeeApproval)
    {
        if (CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot approve a book that is not in the Editing state.");

        if (reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        CommitteeApproval = committeeApproval;

        AddDomainEvent(new Approved(Id, committeeApproval));
    }

    public void SetISBN(ISBN isbn)
    {
        if (CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot approve a book that is not in the Editing state.");

        if (ISBN != null)
            throw new InvalidOperationException(
                "Cannot change already set ISBN.");

        ISBN = isbn;

        AddDomainEvent(new ISBNSet(Id, isbn));
    }

    public void MoveToPrinting(IPublishingHouse publishingHouse)
    {
        if (CurrentState != State.Editing)
            throw new InvalidOperationException("Cannot move to printing from the current state.");

        if (chapters.Count < 1)
            throw new InvalidOperationException("A book must have at least one chapter to move to the printing state.");

        if (CommitteeApproval == null)
            throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

        if (reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");

        if (Genre == null)
            throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

        if (!publishingHouse.IsGenreLimitReached(Genre))
            throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

        CurrentState = State.Printing;

        AddDomainEvent(new MovedToPrinting(Id));
    }

    public void MoveToPublished()
    {
        if (CurrentState != State.Printing || translations.Count < 5)
            throw new InvalidOperationException("Cannot move to Published state from the current state.");

        if (ISBN == null)
            throw new InvalidOperationException("Cannot move to Published state without ISBN.");

        if (reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

        CurrentState = State.Published;

        AddDomainEvent(new Published(Id, ISBN, Title, Author));
    }

    public void MoveToOutOfPrint()
    {
        if (CurrentState != State.Published)
            throw new InvalidOperationException("Cannot move to Out of Print state from the current state.");

        double totalCopies = formats.Sum(f => f.TotalCopies.Value);
        double totalSoldCopies = formats.Sum(f => f.SoldCopies.Value);
        if ((totalSoldCopies / totalCopies) > 0.1)
            throw new InvalidOperationException(
                "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

        CurrentState = State.OutOfPrint;

        AddDomainEvent(new MovedToOutOfPrint(Id));
    }

    private Book(
        BookId bookId,
        State state,
        Title title,
        Author author,
        Genre? genre,
        ISBN? isbn = null,
        CommitteeApproval? committeeApproval = null,
        List<Reviewer>? reviewers = null,
        List<Chapter>? chapters = null,
        List<Translation>? translations = null,
        List<Format>? formats = null
    ): base(bookId)
    {
        CurrentState = state;
        Title = title;
        Author = author;
        Genre = genre;
        ISBN = isbn;
        CommitteeApproval = committeeApproval;
        this.reviewers = reviewers ?? new List<Reviewer>();
        this.chapters = chapters ?? new List<Chapter>();
        this.translations = translations ?? new List<Translation>();
        this.formats = formats ?? new List<Format>();
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
            new Book(
                bookId, state, title, author, genre, isbn, committeeApproval, reviewers, chapters, translations, formats
            );
    }
}
