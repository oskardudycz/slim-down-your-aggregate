namespace PublishingHouse.Persistence.Translators;

public class TranslatorEntity
{
    public TranslatorEntity(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
}

