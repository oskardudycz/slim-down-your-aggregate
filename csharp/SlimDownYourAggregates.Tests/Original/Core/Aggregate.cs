namespace SlimDownYourAggregates.Tests.Original.Core;

public abstract class Aggregate
{
    public Guid Id { get; protected set; }

    private List<object> _domainEvents = new();

    protected Aggregate(Guid id)
    {
        Id = id;
    }

    protected void AddDomainEvent(object domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();
}
