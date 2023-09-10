namespace PublishingHouse.Persistence.Languages;

public class LanguageEntity
{
    public LanguageEntity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
}

