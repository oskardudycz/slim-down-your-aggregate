import { v4 as uuid } from 'uuid';
import { EventEnvelope } from '../../../infrastructure/events';

export type OutboxMessageEntity = {
  readonly position?: number;
  readonly messageId: string;
  readonly messageType: string;
  readonly data: string;
  readonly scheduled: Date;
};

let globalAutoincrementedIdCounter = 0;

export const outboxMessage = (envelope: EventEnvelope) => {
  return {
    // simulate autoincremented id
    position: ++globalAutoincrementedIdCounter,
    messageId: uuid(),
    messageType: envelope.event.type,
    data: JSON.stringify(envelope.event.data),
    scheduled: new Date(),
  };
};
