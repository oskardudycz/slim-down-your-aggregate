import { NonEmptyString } from '#core/typing';
import { Database, EntitiesCollection } from '../../orm';
import { DomainEvent, EventEnvelope } from '../../../infrastructure/events';
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

    this.processEvents(
      this.orm,
      entity,
      events.map((e) => {
        return { event: e, metadata: { recordId: key } };
      }),
    );

    await this.orm.saveChanges();
  }

  private processEvents(
    orm: TOrm,
    current: TEntity | null,
    events: EventEnvelope<TEvent>[],
  ): void {
    const outboxTable = orm.table<OutboxMessageEntity>('outbox');

    for (const eventEnvelope of events) {
      this.evolve(orm, current, eventEnvelope);

      const message = outboxMessage(this.enrich(eventEnvelope, current));
      outboxTable.add(message.position.toString(), message);
    }
  }

  protected abstract evolve(
    orm: TOrm,
    current: TEntity | null,
    event: EventEnvelope<TEvent>,
  ): void;

  protected enrich(
    event: EventEnvelope<TEvent>,
    _current: TEntity | null,
  ): EventEnvelope {
    return event;
  }
}
