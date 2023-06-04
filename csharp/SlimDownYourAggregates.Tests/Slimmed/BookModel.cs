using SlimDownYourAggregates.Tests.Slimmed.Entities;

namespace SlimDownYourAggregates.Tests.Slimmed;

public class BookModel
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public Guid Id { get; protected set; }

    private State CurrentState { get; set; } = State.Writing;
    public BookId BookId { get; }
    public Title Title { get; }

    public Author Author { get; }
    public Genre Genre { get; }
    public List<Reviewer> Reviewers { get; }
    public IReadOnlyList<Chapter> Chapters;
    public CommitteeApproval? CommitteeApproval { get; }
    public Publisher Publisher { get; }
    public ISBN ISBN { get; }
    public DateTime PublicationDate { get; }
    public int Edition { get; }
    public int TotalPages { get; }
    public int NumberOfIllustrations { get; }
    public string BindingType { get; }
    public string Summary { get; }
    public IReadOnlyList<Translation> Translations;
    public IReadOnlyList<Format> Formats;

    public BookModel(
        BookId bookId,
        Title title,
        Author author,
        Genre genre,
        List<Reviewer> reviewers,
        CommitteeApproval? committeeApproval,
        Publisher publisher,
        ISBN isbn,
        DateTime publicationDate,
        int edition,
        int totalPages,
        int numberOfIllustrations,
        string bindingType,
        string summary,
        IReadOnlyList<Chapter> chapters,
        IReadOnlyList<Translation> translations,
        IReadOnlyList<Format> formats
    )
    {
        Id = bookId.Value;
        BookId = bookId;
        Title = title;
        Author = author;
        Genre = genre;
        Reviewers = reviewers;
        CommitteeApproval = committeeApproval;
        Publisher = publisher;
        ISBN = isbn;
        PublicationDate = publicationDate;
        Edition = edition;
        TotalPages = totalPages;
        NumberOfIllustrations = numberOfIllustrations;
        BindingType = bindingType;
        Summary = summary;
        Chapters = chapters;
        Translations = translations;
        Formats = formats;
    }
}
