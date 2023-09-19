import { DomainEvent } from '../../../infrastructure/events';
import { BookId } from '../entities';

export type BookMovedToEditingEvent = DomainEvent<
  'BookMovedToEditingEvent',
  {
    bookId: BookId;
  }
>;
