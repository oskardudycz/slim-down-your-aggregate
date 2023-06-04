using SlimDownYourAggregates.Tests.Slimmed.Entities;
using SlimDownYourAggregates.Tests.Slimmed.Services;
using static SlimDownYourAggregates.Tests.Slimmed.BookEvent;

namespace SlimDownYourAggregates.Tests.Slimmed;

public class Book
{
    private Guid Id => BookId.Value;
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
    )
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

    public ChapterAdded AddChapter(ChapterTitle title, ChapterContent content)
    {
        if (_chapters.Any(chap => chap.Title.Value == title.Value))
            throw new InvalidOperationException($"Chapter with title {title.Value} already exists.");

        if (_chapters.Count > 0 && _chapters.Last().Title.Value != "Chapter " + (_chapters.Count))
            throw new InvalidOperationException(
                $"Chapter should be added in sequence. The title of the next chapter should be 'Chapter {_chapters.Count + 1}'");

        var chapter = new Chapter(title, content);

        return Evolve(new ChapterAdded(BookId, chapter));
    }

    public MovedToEditing MoveToEditing()
    {
        if (_currentState != State.Writing)
            throw new InvalidOperationException("Cannot move to Editing state from the current state.");

        if (_chapters.Count < 1)
            throw new InvalidOperationException("A book must have at least one chapter to move to the Editing state.");

        return Evolve(new MovedToEditing(BookId));
    }


    public TranslationAdded AddTranslation(Translation translation)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot add translation of a book that is not in the Editing state.");

        if (_translations.Count >= 5)
            throw new InvalidOperationException("Cannot add more translations. Maximum 5 translations are allowed.");

        return Evolve(new TranslationAdded(BookId, translation));
    }

    public FormatAdded AddFormat(Format format)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot add format of a book that is not in the Editing state.");

        if (_formats.Any(f => f.FormatType == format.FormatType))
            throw new InvalidOperationException($"Format {format.FormatType} already exists.");

        return Evolve(new FormatAdded(BookId, format));
    }

    public FormatRemoved RemoveFormat(Format format)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot remove format of a book that is not in the Editing state.");

        var existingFormat = _formats.FirstOrDefault(f => f.FormatType == format.FormatType);
        if (existingFormat == null)
            throw new InvalidOperationException($"Format {format.FormatType} does not exist.");

        return Evolve(new FormatRemoved(BookId, format));
    }

    public Approved Approve(CommitteeApproval committeeApproval)
    {
        if (_currentState != State.Editing)
            throw new InvalidOperationException("Cannot approve a book that is not in the Editing state.");

        if (Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be approved unless it has been reviewed by at least three reviewers.");

        return Evolve(new Approved(BookId, committeeApproval));
    }

    public MovedToPrinting MoveToPrinting()
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

        return Evolve(new MovedToPrinting(BookId));
    }

    public Published MoveToPublished()
    {
        if (_currentState != State.Printing || _translations.Count < 5)
            throw new InvalidOperationException("Cannot move to Published state from the current state.");

        if (Reviewers.Count < 3)
            throw new InvalidOperationException(
                "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

        return Evolve(new Published(BookId, ISBN, Title, Author));
    }

    public MovedToOutOfPrint MoveToOutOfPrint()
    {
        if (_currentState != State.Published)
            throw new InvalidOperationException("Cannot move to Out of Print state from the current state.");

        double totalCopies = _formats.Sum(f => f.TotalCopies);
        double totalSoldCopies = _formats.Sum(f => f.SoldCopies);
        if ((totalSoldCopies / totalCopies) > 0.1)
            throw new InvalidOperationException(
                "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

        return Evolve(new MovedToOutOfPrint(BookId));
    }

    public T Evolve<T>(T @event) where T : BookEvent
    {
        switch(@event)
        {
            case ChapterAdded chapterAdded:
            {
                _chapters.Add(chapterAdded.Chapter);
                return @event;
            }
            case MovedToEditing ignore: {
                _currentState = State.Editing;
                return @event;
            }
            case FormatAdded formatAdded: {
                _formats.Add(formatAdded.Format);
                return @event;
            }
            case FormatRemoved formatRemoved: {
                var existingFormat = _formats.FirstOrDefault(f => f.FormatType == formatRemoved.Format.FormatType);
                if (existingFormat != null)
                    _formats.Remove(existingFormat);

                return @event;
            }
            case TranslationAdded translationAdded: {
                _translations.Add(translationAdded.Translation);
                return @event;
            }
            case Approved approved: {
                _committeeApproval = approved.CommitteeApproval;
                return @event;
            }
            case MovedToPrinting ignore: {
                _currentState = State.Editing;
                return @event;
            }
            case Published ignore: {
                _currentState = State.Printing;
                return @event;
            }
            case MovedToOutOfPrint ignore: {
                _currentState = State.OutOfPrint;
                return @event;
            }
            default:
            {
                return @event;
            }
        }
    }
}
