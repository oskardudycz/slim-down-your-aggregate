import { DomainEvent } from 'src/original/infrastructure/events';
import { BookId, Chapter } from '../entities';

export type ChapterAddedEvent = DomainEvent<
  'ChapterAddedEvent',
  {
    bookId: BookId;
    chapter: Chapter;
  }
>;
