using SlimDownYourAggregates.Tests.Slimmed.Core;
using SlimDownYourAggregates.Tests.Slimmed.Entities;
using SlimDownYourAggregates.Tests.Slimmed.Events;
using SlimDownYourAggregates.Tests.Slimmed.Services;

namespace SlimDownYourAggregates.Tests.Slimmed;

public class Book: Aggregate
{
    private List<Chapter> _chapters = new();
    private CommitteeApproval? _committeeApproval;
    private readonly IPublishingHouse _publishingHouse;
    private readonly List<Translation> _translations = new();
    private readonly List<Format> _formats = new();

    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    private State _currentState = State.Writing;

    public Book(
        BookId bookId,
        Title title,
        Author author,
        Genre genre,
        List<Reviewer> reviewers,
        IPublishingHouse publishingHouse,
        ISBN isbn
    ): base(bookId.Value)
    {
        BookId = bookId;
        Title = title;
        Author = author;
        Genre = genre;
        Reviewers = reviewers;
        _publishingHouse = publishingHouse;
        ISBN = isbn;
    }

    public BookId BookId { get; }
    public Title Title { get; }

    public Author Author { get; }
    public Genre Genre { get; }
    public List<Reviewer> Reviewers { get; }
    public ISBN ISBN { get; }

    public void AddChapter(ChapterTitle title, ChapterContent content)
    {
        if (_chapters.Any(chap => chap.Title.Value == title.Value))
            throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

        if (_chapters.Count > 0 && _chapters.Last().Title.Value != "Chapter " + (_chapters.Count))
            throw new InvalidOperationException(
                $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {_chapters.Count + 1}'");

        var chapter = new Chapter(title, content);
        _chapters.Add(chapter);

        AddDomainEvent(new ChapterAddedEvent(Id, chapter));
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

    public void MoveToEditing()
    {
        if (_currentState != State.Writing)
            throw new InvalidOperationException("Cannot move to Editing state from the current state.");

        if (_chapters.Count < 1)
            throw new InvalidOperationException("A book must have at least one chapter to move to the Editing state.");

        _currentState = State.Editing;

        AddDomainEvent(new BookMovedToEditingEvent(Id));
    }

    public void MoveToPrinting()
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot move to Printing state from the current state.");

        if (_committeeApproval == null)
            throw new InvalidOperationException("Cannot move to the Printing state until the book has been approved.");

        if (Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");

        if (!_publishingHouse.IsGenreLimitReached(Genre))
            throw new InvalidOperationException("Cannot move to the Printing state until the genre limit is reached.");

        _currentState = State.Printing;
    }


    public void MoveToPublished()
    {
        if (_currentState != State.Printing || _translations.Count < 5)
            throw new InvalidOperationException("Cannot move to Published state from the current state.");

        if (Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

        _currentState = State.Published;

        AddDomainEvent(new BookPublishedEvent(Id, ISBN, Title, Author));
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

    public void AddTranslation(Translation translation)
    {
        if (_translations.Count >= 5)
            throw new InvalidOperationException("Cannot add more translations. Maximum 5 translations are allowed.");

        _translations.Add(translation);
    }

    public void AddFormat(Format format)
    {
        if (_formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        _formats.Add(format);
    }

    public void RemoveFormat(string formatType)
    {
        var format = _formats.FirstOrDefault(f => f.FormatType == formatType);
        if (format == null)
            throw new InvalidOperationException($"Format {formatType} does not exist.");

        _formats.Remove(format);
    }
}
