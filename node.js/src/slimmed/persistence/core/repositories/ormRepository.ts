import { NonEmptyString } from '#core/typing';
import { Aggregate } from '../../../infrastructure/aggregates';
import { Database, EntitiesCollection } from '../../orm';
import { DomainEvent } from '../../../infrastructure/events';
import {
  OutboxMessageEntity,
  outboxMessage,
} from '../outbox/outboxMessageEntity';

export abstract class OrmRepository<
  TAggregate extends Aggregate<TKey>,
  TKey extends NonEmptyString,
  TEntity,
  TOrm extends Database,
> {
  protected constructor(
    protected readonly orm: TOrm,
    protected readonly entities: EntitiesCollection<TEntity>,
  ) {}

  async findById(key: TKey): Promise<TAggregate | null> {
    const entity = await this.entities.findById(key);

    return entity != null ? this.mapToAggregate(entity) : null;
  }

  public async add(aggregate: TAggregate): Promise<void> {
    this.entities.add(aggregate.id, this.mapToEntity(aggregate));

    this.scheduleOutbox(aggregate.domainEvents);

    await this.orm.saveChanges();
    aggregate.clearEvents();
  }

  public async update(aggregate: TAggregate): Promise<void> {
    const id: string = aggregate.id;

    const entity = await this.entities.findById(id);

    if (!entity) throw new Error(`Entity with id ${id} was not found`);

    this.entities.update(id, this.mapToEntity(aggregate));

    this.scheduleOutbox(aggregate.domainEvents);

    await this.orm.saveChanges();
    aggregate.clearEvents();
  }

  private scheduleOutbox(events: DomainEvent[]): void {
    const outboxTable = this.orm.table<OutboxMessageEntity>('outbox');

    const messages = events.map(outboxMessage);

    for (const message of messages) {
      outboxTable.add(message.position.toString(), message);
    }
  }

  protected abstract mapToAggregate(entity: TEntity): TAggregate;

  protected abstract mapToEntity(aggregate: TAggregate): TEntity;
}
