using Ogooreck.API;
using Xunit;
using static Ogooreck.API.ApiSpecification;

namespace PublishingHouse.Api.Tests.Books;

public class CreateDraftTests: IClassFixture<ApiSpecification<Program>>
{
    [Fact]
    public Task DraftIsCreated_ForValidRequest() =>
        API.Given()
            .When(
                POST,
                URI("/api/ShoppingCarts/")
                //BODY(new OpenShoppingCartRequest(ClientId))
            )
            .Then(CREATED);

    private readonly ApiSpecification<Program> API;

    public CreateDraftTests(ApiSpecification<Program> api) =>
        API = api;
}
