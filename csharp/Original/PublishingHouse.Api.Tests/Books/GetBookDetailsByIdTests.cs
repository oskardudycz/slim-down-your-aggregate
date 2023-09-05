namespace PublishingHouse.Api.Tests.Books;

public class GetBookDetailsByIdTests: IClassFixture<ApiSpecification<Program>>
{
    [Fact]
    public Task ReturnsNotFound_ForNonExistingBook() =>
        API.Given()
            .When(
                GET,
                URI($"/api/books/{UnknownBookId}")
            )
            .Then(NOT_FOUND);

    [Fact]
    public Task ReturnsDetails_ForExistingBook() =>
        API.Given(CreatedDraft())
            .When(
                GET,
                URI(ctx => $"/api/books/{ctx.GetCreatedId()}")
            )
            .Then(OK);

    private readonly Guid UnknownBookId = Guid.NewGuid();
    private readonly Guid ExistingBookId = Guid.NewGuid();

    private readonly ApiSpecification<Program> API;

    public GetBookDetailsByIdTests(ApiSpecification<Program> api) =>
        API = api;
}
