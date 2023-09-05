namespace PublishingHouse.Api.Tests.Books;

public class CreateDraftTests: IClassFixture<ApiSpecification<Program>>
{
    [Fact]
    public Task DraftIsCreated_ForValidRequest() =>
        API.Given()
            .When(
                POST,
                URI("/api/books/")
                //BODY(new OpenShoppingCartRequest(ClientId))
            )
            .Then(
                CREATED_WITH_DEFAULT_HEADERS(locationHeaderPrefix: "/api/books/"),
                (response, _) =>
                {
                    response.TryGetCreatedId<Guid>(out var createdId).Should().BeTrue();
                    createdId.Should().NotBeEmpty();

                    return ValueTask.CompletedTask;
                });

    private readonly ApiSpecification<Program> API;

    public CreateDraftTests(ApiSpecification<Program> api) =>
        API = api;
}
