namespace PublishingHouse.Core.Aggregates;

public abstract class Aggregate<TKey>
{
    public TKey Id { get; }

    private readonly List<IDomainEvent> domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => domainEvents.AsReadOnly();

    protected Aggregate(TKey id) =>
        Id = id;

    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        domainEvents.Add(domainEvent);

    public void ClearEvents() =>
        domainEvents.Clear();
}
