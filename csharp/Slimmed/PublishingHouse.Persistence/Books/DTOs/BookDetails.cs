namespace PublishingHouse.Persistence.Books.DTOs;

using static BookDetails;

public record BookDetails(
    Guid Id,
    string CurrentState,
    string Title,
    AuthorDetails Author,
    string PublisherName,
    int Edition,
    string? Genre,
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
    IReadOnlyList<FormatDetails> Formats
)
{
    public record AuthorDetails(string FirstName, string LastName);
    public record CommitteeApprovalDetails(bool IsApproved, string Feedback);
    public record ChapterDetails(string Title, string? Content);
    public record TranslationDetails(string Language, string Translator);
    public record FormatDetails(string FormatType, int TotalCopies, int SoldCopies);
}
