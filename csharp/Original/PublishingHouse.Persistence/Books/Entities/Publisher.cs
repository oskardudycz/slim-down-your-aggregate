namespace PublishingHouse.Persistence.Books.Entities;

public class Publisher
{
    public Publisher(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
}

