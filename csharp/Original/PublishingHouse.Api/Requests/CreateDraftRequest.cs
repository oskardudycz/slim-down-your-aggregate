namespace PublishingHouse.Api.Requests;

public record CreateDraftRequest(
    string? Title,
    AuthorRequest? Author,
    Guid? PublisherId,
    int? Edition,
    string? Genre
);

public record AuthorRequest(Guid? AuthorId, string? FirstName, string? LastName);
