namespace PublishingHouse.Persistence.Books.Entities;

public class Reviewer
{
    public Reviewer(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
}

