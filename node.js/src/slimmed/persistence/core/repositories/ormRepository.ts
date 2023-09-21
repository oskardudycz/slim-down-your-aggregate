import { NonEmptyString } from '#core/typing';
import { Aggregate } from '../../../infrastructure/aggregates';
import { Database, EntitiesCollection } from '../../orm';
import { DomainEvent } from '../../../infrastructure/events';
import {
  OutboxMessageEntity,
  outboxMessage,
} from '../outbox/outboxMessageEntity';

export abstract class OrmRepository<
  TAggregate extends Aggregate<TKey, TEvent>,
  TKey extends NonEmptyString,
  TEvent extends DomainEvent,
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

  public async store(key: TKey, events: TEvent[]): Promise<void> {
    const entity = await this.entities.findById(key);

    this.processEvents(this.orm, entity ?? key, events);

    await this.orm.saveChanges();
  }

  private processEvents(
    orm: TOrm,
    current: TEntity | TKey,
    events: TEvent[],
  ): void {
    const outboxTable = orm.table<OutboxMessageEntity>('outbox');

    for (const event of events) {
      this.evolve(orm, current, event);

      const message = outboxMessage(event);
      outboxTable.add(message.position.toString(), message);
    }
  }

  protected abstract mapToAggregate(entity: TEntity): TAggregate;

  protected abstract evolve(
    orm: TOrm,
    current: TEntity | TKey,
    event: TEvent,
  ): void;
}
