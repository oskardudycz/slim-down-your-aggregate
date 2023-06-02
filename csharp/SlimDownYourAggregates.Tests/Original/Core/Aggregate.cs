namespace SlimDownYourAggregates.Tests.Original.Core;

public class Aggregate
{
    public Guid Id { get; protected set; }

    private List<IDomainEvent> _domainEvents = new();

    protected Aggregate(Guid id)
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
