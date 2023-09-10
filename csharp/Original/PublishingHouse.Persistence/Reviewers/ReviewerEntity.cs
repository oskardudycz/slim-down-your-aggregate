namespace PublishingHouse.Persistence.Reviewers;

public class ReviewerEntity
{
    public ReviewerEntity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
}

