using SlimDownYourAggregates.Tests.Original.Entities;

namespace SlimDownYourAggregates.Tests.Original;

public class BookModel
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public Guid Id { get; set; }

    public State CurrentState { get; set; } = State.Writing;
    public BookId BookId { get; set; }
    public Title Title { get; set; }

    public Author Author { get; set; }
    public Genre Genre { get; set; }
    public List<Reviewer> Reviewers { get; set; }
    public List<Chapter> Chapters { get; set; }
    public CommitteeApproval? CommitteeApproval { get; set; }
    public Publisher Publisher { get; set; }
    public ISBN ISBN { get; set; }
    public DateTime PublicationDate { get; set; }
    public int Edition { get; set; }
    public int TotalPages { get; set; }
    public int NumberOfIllustrations { get; set; }
    public string BindingType { get; set; }
    public string Summary { get; set; }
    public IReadOnlyList<Translation> Translations { get; set; }
    public IReadOnlyList<Format> Formats { get; set; }

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
