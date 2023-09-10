namespace PublishingHouse.Persistence.Authors;

public class AuthorEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
