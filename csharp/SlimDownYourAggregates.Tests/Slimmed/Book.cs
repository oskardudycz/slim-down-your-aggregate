using SlimDownYourAggregates.Tests.Slimmed.Entities;
using SlimDownYourAggregates.Tests.Slimmed.Services;
using static SlimDownYourAggregates.Tests.Slimmed.BookEvent;

namespace SlimDownYourAggregates.Tests.Slimmed;

public class Book
{
    private Guid Id => BookId.Value;
    public List<Chapter> Chapters { get; } = new();
    public CommitteeApproval? CommitteeApproval { get; set; }
    public IPublishingHouse PublishingHouse { get; }
    public List<Translation> Translations { get; } = new();
    public List<Format> Formats { get; } = new();

    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public State CurrentState { get; private set; } = State.Writing;

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
        PublishingHouse = publishingHouse;
        ISBN = isbn;
    }

    public BookId BookId { get; }
    public Title Title { get; }

    public Author Author { get; }
    public Genre Genre { get; }
    public List<Reviewer> Reviewers { get; }
    public ISBN ISBN { get; }

    public T Evolve<T>(T @event) where T : BookEvent
    {
        switch(@event)
        {
            case ChapterAdded chapterAdded:
            {
                Chapters.Add(chapterAdded.Chapter);
                return @event;
            }
            case MovedToEditing ignore: {
                CurrentState = State.Editing;
                return @event;
            }
            case FormatAdded formatAdded: {
                Formats.Add(formatAdded.Format);
                return @event;
            }
            case FormatRemoved formatRemoved: {
                var existingFormat = Formats.FirstOrDefault(f => f.FormatType == formatRemoved.Format.FormatType);
                if (existingFormat != null)
                    Formats.Remove(existingFormat);

                return @event;
            }
            case TranslationAdded translationAdded: {
                Translations.Add(translationAdded.Translation);
                return @event;
            }
            case Approved approved: {
                CommitteeApproval = approved.CommitteeApproval;
                return @event;
            }
            case MovedToPrinting ignore: {
                CurrentState = State.Editing;
                return @event;
            }
            case Published ignore: {
                CurrentState = State.Printing;
                return @event;
            }
            case MovedToOutOfPrint ignore: {
                CurrentState = State.OutOfPrint;
                return @event;
            }
            default:
            {
                return @event;
            }
        }
    }
}
