using Microsoft.EntityFrameworkCore;
using PublishingHouse.Core.Aggregates;
using PublishingHouse.Core.ValueObjects;
using PublishingHouse.Persistence.Core.Outbox;

namespace PublishingHouse.Persistence.Core.Repositories;

public abstract class EntityFrameworkRepository<TAggregate, TKey, TEvent, TEntity,  TDbContext>
    where TAggregate: Aggregate<TKey, TEvent>
    where TKey: NonEmptyGuid
    where TEvent: class
    where TDbContext: DbContext
    where TEntity : class
{
    protected readonly TDbContext DbContext;

    protected EntityFrameworkRepository(TDbContext dbContext) =>
        DbContext = dbContext;

    public async Task<TAggregate?> FindById(TKey bookId, CancellationToken ct)
    {
        var entity = await Includes(DbContext.Set<TEntity>())
            .AsNoTracking()
            .SingleOrDefaultAsync(ct);

        return entity != null ? MapToAggregate(entity): default;
    }

    public async Task Store(TAggregate aggregate, CancellationToken ct)
    {
        var entity = await DbContext.Set<TEntity>().FindAsync(
            new object?[] { aggregate.Id.Value },
            cancellationToken: ct
        ) ?? null;

        ProcessEvents(DbContext, entity, aggregate.DomainEvents);

        await DbContext.SaveChangesAsync(ct);
        aggregate.ClearEvents();
    }

    private void ProcessEvents(TDbContext dbContext, TEntity? entity, IReadOnlyList<TEvent> events)
    {
        var outbox = dbContext.Set<OutboxMessageEntity>();
        foreach (var @event in events)
        {
            Evolve(dbContext, entity, @event);
            outbox.Add(OutboxMessageEntity.From(@event));
        }
    }

    protected virtual IQueryable<TEntity> Includes(DbSet<TEntity> query) =>
        query;

    protected abstract TAggregate MapToAggregate(TEntity entity);

    protected abstract void Evolve(TDbContext dbContext, TEntity? current, TEvent @event);
}
