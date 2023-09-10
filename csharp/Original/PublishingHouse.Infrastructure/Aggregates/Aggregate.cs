namespace PublishingHouse.Core.Aggregates;

public abstract class Aggregate<TKey>
{
    public TKey Id { get; protected set; }

    private List<IDomainEvent> _domainEvents = new();

    protected Aggregate(TKey id)
    {
        Id = id;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
}
