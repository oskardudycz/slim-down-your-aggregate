using PublishingHouse.Persistence.Authors;
using PublishingHouse.Persistence.Books.Entities;
using PublishingHouse.Persistence.Books.ValueObjects;
using PublishingHouse.Persistence.Publishers;
using PublishingHouse.Persistence.Reviewers;

namespace PublishingHouse.Persistence.Books;

public class BookEntity
{
    public enum State { Writing, Editing, Printing, Published, OutOfPrint }

    public required Guid Id { get; set; }
    public required State CurrentState { get; set; }
    public required string Title { get; set; }
    public required AuthorEntity Author { get; set; }
    public string? Genre { get; set; }
    public required PublisherEntity Publisher { get; set; }
    public int Edition { get; set; }
    public string? ISBN { get; set; }
    public DateOnly? PublicationDate { get; set; }
    public int? TotalPages { get; set; }
    public int? NumberOfIllustrations { get; set; }
    public string? BindingType { get; set; }
    public string? Summary { get; set; }
    public CommitteeApproval? CommitteeApproval { get; set; }
    public required List<ReviewerEntity> Reviewers { get; set; } = new();
    public required List<ChapterEntity> Chapters { get; set; } = new();
    public required List<Translation> Translations { get; set; } = new();
    public required List<Format> Formats { get; set; } = new();
}
