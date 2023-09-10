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
                     ExistingPublisherId,
                     Faker.Random.Int(0),
                     Faker.Random.String()
                 )
            )
        );

    public static readonly Guid ExistingPublisherId = new("c528d322-17eb-47ba-bccf-6cb61d340f09");

    public static readonly Faker Faker = new();
}
