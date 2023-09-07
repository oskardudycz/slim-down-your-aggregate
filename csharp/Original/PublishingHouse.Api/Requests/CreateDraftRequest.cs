using PublishingHouse.Application.Books.Commands;

namespace PublishingHouse.Api.Requests;

public record CreateDraftRequest(
    string? Title,
    AuthorRequest? Author,
    string? Publisher,
    int? Edition,
    string? Genre
);

public record AuthorRequest(string? FirstName, string? LastName);
