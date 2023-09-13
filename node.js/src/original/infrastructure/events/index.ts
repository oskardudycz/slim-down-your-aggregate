import { DeepReadonly } from 'ts-essentials';

export type DomainEvent<
  EventType extends string = string,
  EventData extends Record<string, unknown> = Record<string, unknown>,
> = DeepReadonly<{
  type: EventType;
  data: EventData;
}>;
