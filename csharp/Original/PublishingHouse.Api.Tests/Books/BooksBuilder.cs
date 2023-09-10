using Bogus;
using PublishingHouse.Api.Requests;

namespace PublishingHouse.Api.Tests.Books;

public static class BooksBuilder
{
    public static RequestDefinition CreatedDraft(CreateDraftRequest? body = null) =>
        SEND(
            "Create Book Draft",
            POST,
            URI("/api/books"),
            BODY(body ??
                 new CreateDraftRequest(
                     Faker.Random.String(),
                     new AuthorRequest(null, Faker.Name.FirstName(), Faker.Name.LastName()),
                     Faker.Random.Guid(),
                     Faker.Random.Int(0),
                     Faker.Random.String()
                 )
            )
        );

    public static readonly Faker Faker = new();
}
