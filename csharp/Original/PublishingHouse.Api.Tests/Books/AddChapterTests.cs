using PublishingHouse.Api.Requests;

namespace PublishingHouse.Api.Tests.Books;

public class AddChapterTests
{
    [Fact]
    public Task AddsChapter_ForExistingBook() =>
        API.Given(CreatedDraft())
            .When(
                POST,
                URI(ctx => $"/api/books/{ctx.GetCreatedId()}/chapters"),
                BODY(new AddChapterRequest("Chapter 1 - Prolog", null))
            )
            .Then(NO_CONTENT);

    private readonly ApiSpecification API = ApiSpecification.WithSchema(nameof(AddChapterTests));
}
