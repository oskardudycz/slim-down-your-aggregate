import { Flavour, NonEmptyString } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type EmptyData = Record<string, never>;

export type DomainEvent<
  EventType extends string = string,
  EventData extends Record<string, unknown> = Record<string, unknown>,
> = Flavour<
  DeepReadonly<{
    type: EventType;
    data: EventData;
  }>,
  'Event'
>;

export type EventEnvelope<
  DomainEventType extends DomainEvent = DomainEvent,
  TKey extends NonEmptyString = NonEmptyString,
> = { event: DomainEventType; metadata: { recordId: TKey } };
