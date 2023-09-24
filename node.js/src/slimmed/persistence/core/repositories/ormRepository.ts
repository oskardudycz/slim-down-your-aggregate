import { NonEmptyString } from '#core/typing';
import { Database, EntitiesCollection } from '../../orm';
import { DomainEvent } from '../../../infrastructure/events';
import {
  OutboxMessageEntity,
  outboxMessage,
} from '../outbox/outboxMessageEntity';

export abstract class OrmRepository<
  TEntity,
  TKey extends NonEmptyString,
  TEvent extends DomainEvent,
  TOrm extends Database,
> {
  protected constructor(
    protected readonly orm: TOrm,
    protected readonly entities: EntitiesCollection<TEntity>,
  ) {}

  public async getAndUpdate(
    key: TKey,
    handle: (entity: TEntity | null) => TEvent[],
  ) {
    const entity = await this.entities.findById(key);

    const events = handle(entity);

    this.processEvents(this.orm, entity, events);

    await this.orm.saveChanges();
  }

  private processEvents(
    orm: TOrm,
    current: TEntity | null,
    events: TEvent[],
  ): void {
    const outboxTable = orm.table<OutboxMessageEntity>('outbox');

    for (const event of events) {
      this.evolve(orm, current, event);

      const message = outboxMessage(this.enrich(event, current));
      outboxTable.add(message.position.toString(), message);
    }
  }

  protected abstract evolve(
    orm: TOrm,
    current: TEntity | null,
    event: TEvent,
  ): void;

  protected enrich(event: TEvent, _current: TEntity | null): DomainEvent {
    return event;
  }
}
