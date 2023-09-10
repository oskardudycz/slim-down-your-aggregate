namespace PublishingHouse.Persistence.Publishers;

public class PublisherEntity
{
    public PublisherEntity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
}

