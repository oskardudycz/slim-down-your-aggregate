import { DomainEvent } from 'src/original/infrastructure/events';
import { Author, BookId, ISBN, Title } from '../entities';

export type BookPublishedEvent = DomainEvent<
  'BookPublishedEvent',
  {
    bookId: BookId;
    isbn: ISBN;
    title: Title;
    author: Author;
  }
>;
