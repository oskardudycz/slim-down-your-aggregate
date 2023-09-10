namespace PublishingHouse.Persistence.Authors;

public class AuthorEntity
{
    public AuthorEntity(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
}
