using PublishingHouse.Books.Entities;
using PublishingHouse.Books.Events;
using PublishingHouse.Books.Services;
using PublishingHouse.Core;

namespace PublishingHouse.Books;

public class Book: Aggregate
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    private readonly List<Chapter> _chapters = new();
    private CommitteeApproval? _committeeApproval;
    private readonly IPublishingHouse _publishingHouse;
    private readonly List<Translation> _translations = new();
    private readonly List<Format> _formats = new();
    private State _currentState = State.Writing;

    public BookId BookId { get; }
    public Title Title { get; }
    public Author Author { get; }
    public Genre? Genre { get; }
    public Publisher Publisher { get; }
    public int Edition { get; }
    public ISBN? ISBN { get; }
    public DateTime? PublicationDate { get; }
    public int? TotalPages { get; }
    public int? NumberOfIllustrations { get; }
    public string? BindingType { get; }

    //TODO: add type for that
    public string? Summary { get; }

    public List<Reviewer> Reviewers { get; }
    public IReadOnlyList<Chapter> Chapters => _chapters.AsReadOnly();
    public CommitteeApproval? CommitteeApproval => _committeeApproval;
    public IReadOnlyList<Translation> Translations => _translations.AsReadOnly();
    public IReadOnlyList<Format> Formats => _formats.AsReadOnly();

    public Book(
        BookId bookId,
        Title title,
        Author author,
        Genre genre,
        List<Reviewer> reviewers,
        IPublishingHouse publishingHouse,
        Publisher publisher,
        ISBN isbn,
        DateTime publicationDate,
        int edition,
        int totalPages,
        int numberOfIllustrations,
        string bindingType,
        string summary
    ): base(bookId.Value)
    {
        BookId = bookId;
        Title = title;
        Author = author;
        Genre = genre;
        Reviewers = reviewers;
        _publishingHouse = publishingHouse;
        Publisher = publisher;
        ISBN = isbn;
        PublicationDate = publicationDate;
        Edition = edition;
        TotalPages = totalPages;
        NumberOfIllustrations = numberOfIllustrations;
        BindingType = bindingType;
        Summary = summary;
    }

    public void AddChapter(ChapterTitle title, ChapterContent content)
    {
        if (_chapters.Any(chap => chap.Title.Value == title.Value))
            throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

        if (_chapters.Count > 0 && _chapters.Last().Title.Value != "Chapter " + (_chapters.Count))
            throw new InvalidOperationException(
                $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {_chapters.Count + 1}'");

        var chapter = new Chapter(title, content);
        _chapters.Add(chapter);

        AddDomainEvent(new ChapterAddedEvent(BookId, chapter));
    }

    public void MoveToEditing()
    {
        if (_currentState != State.Writing)
            throw new InvalidOperationException("Cannot move to Editing state from the current state.");

        if (_chapters.Count < 1)
            throw new InvalidOperationException("A book must have at least one chapter to move to the Editing state.");

        if (Genre == null)
            throw new InvalidOperationException("Book can be moved to the editing only when genre is specified");

        _currentState = State.Editing;

        AddDomainEvent(new BookMovedToEditingEvent(BookId));
    }

    public void AddTranslation(Translation translation)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot add translation of a book that is not in the Editing state.");

        if (_translations.Count >= 5)
            throw new InvalidOperationException("Cannot add more translations. Maximum 5 translations are allowed.");

        _translations.Add(translation);
    }

    public void AddFormat(Format format)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot add format of a book that is not in the Editing state.");

        if (_formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        _formats.Add(format);
    }

    public void RemoveFormat(Format format)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot remove format of a book that is not in the Editing state.");

        var existingFormat = _formats.FirstOrDefault(f => f.FormatType == format.FormatType);
        if (existingFormat == null)
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        _formats.Remove(existingFormat);
    }

    public void Approve(CommitteeApproval committeeApproval)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot approve a book that is not in the Editing state.");

        if (Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        _committeeApproval = committeeApproval;
    }

    public void MoveToPrinting()
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot move to printing from the current state.");

        if (_committeeApproval == null)
            throw new InvalidOperationException("Cannot move to printing state until the book has been approved.");

        if (Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");

        if (Genre == null)
            throw new InvalidOperationException("Book can be moved to the printing only when genre is specified");

        if (!_publishingHouse.IsGenreLimitReached(Genre))
            throw new InvalidOperationException("Cannot move to printing until the genre limit is reached.");

        _currentState = State.Printing;
    }

    public void MoveToPublished()
    {
        if (_currentState != State.Printing || _translations.Count < 5)
            throw new InvalidOperationException("Cannot move to Published state from the current state.");

        if (ISBN == null)
            throw new InvalidOperationException("Cannot move to Published state without ISBN.");

        if (Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

        _currentState = State.Published;

        AddDomainEvent(new BookPublishedEvent(BookId, ISBN, Title, Author));
    }

    public void MoveToOutOfPrint()
    {
        if (_currentState != State.Published)
            throw new InvalidOperationException("Cannot move to Out of Print state from the current state.");

        double totalCopies = _formats.Sum(f => f.TotalCopies);
        double totalSoldCopies = _formats.Sum(f => f.SoldCopies);
        if ((totalSoldCopies / totalCopies) > 0.1)
            throw new InvalidOperationException(
                "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

        _currentState = State.OutOfPrint;
    }
}
