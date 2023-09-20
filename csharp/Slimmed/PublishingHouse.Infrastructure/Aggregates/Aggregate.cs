namespace PublishingHouse.Core.Aggregates;

public abstract class Aggregate<TKey, TEvent>
{
    public TKey Id { get; }

    private readonly List<TEvent> domainEvents = new();

    public IReadOnlyList<TEvent> DomainEvents
        => domainEvents.AsReadOnly();

    protected Aggregate(TKey id) =>
        Id = id;

    protected void AddDomainEvent(TEvent domainEvent) =>
        domainEvents.Add(domainEvent);

    public void ClearEvents() =>
        domainEvents.Clear();
}
