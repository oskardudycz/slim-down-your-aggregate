import { v4 as uuid } from 'uuid';
import { DomainEvent } from 'src/original/infrastructure/events';

export type OutboxMessageEntity = {
  readonly position?: number;
  readonly messageId: string;
  readonly messageType: string;
  readonly data: string;
  readonly scheduled: Date;
};

let globalAutoincrementedIdCounter = 0;

export const outboxMessage = (message: DomainEvent) => {
  return {
    // simulate autoincremented id
    position: ++globalAutoincrementedIdCounter,
    messageId: uuid(),
    messageType: message.type,
    data: JSON.stringify(message.data),
    scheduled: new Date(),
  };
};
