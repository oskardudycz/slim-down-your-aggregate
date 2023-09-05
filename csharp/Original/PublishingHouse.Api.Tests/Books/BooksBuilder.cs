using PublishingHouse.Api.Requests;

namespace PublishingHouse.Api.Tests.Books;

public static class BooksBuilder
{
    public static RequestDefinition CreatedDraft(CreateDraftRequest? body = null) =>
        SEND(
            "Create Book Draft",
            POST,
            URI("/api/books"),
            BODY(body ?? new CreateDraftRequest())
        );
}
