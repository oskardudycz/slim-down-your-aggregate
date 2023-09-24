using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PublishingHouse.Core.ValueObjects;
using PublishingHouse.Persistence.Core.Outbox;

namespace PublishingHouse.Persistence.Core.Repositories;

public abstract class EntityFrameworkRepository<TEntity,  TKey, TEvent, TDbContext>
    where TEntity : class
    where TKey: NonEmptyGuid
    where TEvent: class
    where TDbContext: DbContext
{
    protected readonly TDbContext DbContext;
    private readonly Func<TKey, Expression<Func<TEntity, bool>>> getId;

    protected EntityFrameworkRepository(TDbContext dbContext, Func<TKey, Expression<Func<TEntity, bool>>> getId)
    {
        DbContext = dbContext;
        this.getId = getId;
    }

    public async Task GetAndUpdate(TKey id, Func<TEntity?, TEvent[]> handle, CancellationToken ct)
    {
        var entity = await Includes(DbContext.Set<TEntity>())
            .SingleOrDefaultAsync(getId(id), ct);

        var events = handle(entity);

        ProcessEvents(DbContext, entity, events);

        await DbContext.SaveChangesAsync(ct);
    }

    private void ProcessEvents(TDbContext dbContext, TEntity? entity, IReadOnlyList<TEvent> events)
    {
        var outbox = dbContext.Set<OutboxMessageEntity>();
        foreach (var @event in events)
        {
            Evolve(dbContext, entity, @event);
            outbox.Add(OutboxMessageEntity.From(Enrich(@event, entity)));
        }
    }

    protected virtual IQueryable<TEntity> Includes(DbSet<TEntity> query) =>
        query;

    protected abstract void Evolve(TDbContext dbContext, TEntity? current, TEvent @event);

    protected virtual object Enrich(TEvent @event, TEntity? _) => @event;
}
