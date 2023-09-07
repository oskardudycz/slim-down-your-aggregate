using PublishingHouse.Persistence.Books.Entities;

namespace PublishingHouse.Persistence.Books;

public class BookEntity
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public required Guid Id { get; set; }
    public required State CurrentState { get; set; }
    public required string Title { get; set; }
    public required Author Author { get; set; }
    public string? Genre { get; set; }
    public required string Publisher { get; set; }
    public int Edition { get; set; }
    public string? ISBN { get; set; }
    public DateTime? PublicationDate { get; set; }
    public int? TotalPages { get; set; }
    public int? NumberOfIllustrations { get; set; }
    public string? BindingType { get; set; }
    public string? Summary { get; set; }
    public CommitteeApproval? CommitteeApproval { get; set; }
    public required List<Reviewer> Reviewers { get; set; }
    public required IReadOnlyList<Chapter> Chapters { get; set; }
    public required IReadOnlyList<Translation> Translations { get; set; }
    public required IReadOnlyList<Format> Formats { get; set; }
}
