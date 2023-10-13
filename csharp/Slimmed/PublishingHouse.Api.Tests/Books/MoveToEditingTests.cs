namespace PublishingHouse.Api.Tests.Books;

public class MoveToEditingTests: IClassFixture<ApiSpecification>
{
    [Fact]
    public Task MoveToEditing_ForExistingBook() =>
        API.Given(
                CreatedDraft(),
                WithChapter()
            )
            .When(
                POST,
                URI(ctx => $"/api/books/{ctx.GetCreatedId()}/move-to-editing")
            )
            .Then(NO_CONTENT);

    private readonly ApiSpecification API;

    public MoveToEditingTests(ApiSpecification api) =>
        API = api;
}
