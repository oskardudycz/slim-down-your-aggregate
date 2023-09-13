import { Flavour } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

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
