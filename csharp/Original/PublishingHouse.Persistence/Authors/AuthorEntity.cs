namespace PublishingHouse.Persistence.Authors;

public class AuthorEntity
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
