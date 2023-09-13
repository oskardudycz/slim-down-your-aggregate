import { DomainEvent } from 'src/original/infrastructure/events';
import { BookId } from '../entities';

export type BookMovedToEditingEvent = DomainEvent<
  'BookMovedToEditingEvent',
  {
    bookId: BookId;
  }
>;
