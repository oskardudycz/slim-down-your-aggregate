namespace PublishingHouse.Books.DTOs;

using static BookDetails;

public record BookDetails(
    Guid Id,
    string CurrentState,
    string Title,
    AuthorDetails Author,
    string? Genre,
    string PublisherName,
    int Edition,
    string? ISBN,
    DateOnly? PublicationDate,
    int? TotalPages,
    int? NumberOfIllustrations,
    string? BindingType,
    string? Summary,
    CommitteeApprovalDetails? CommitteeApproval,
    List<string> Reviewers,
    IReadOnlyList<ChapterDetails> Chapters,
    IReadOnlyList<TranslationDetails> Translations,
    IReadOnlyList<Format> Formats
)
{
    public record AuthorDetails(string FirstName, string LastName);
    public record CommitteeApprovalDetails(bool IsApproved, string Feedback);
    public record ChapterDetails(string Title, string Content);
    public record TranslationDetails(string Language, string Translator);
    public record Format(string FormatType, int TotalCopies, int SoldCopies);
}
