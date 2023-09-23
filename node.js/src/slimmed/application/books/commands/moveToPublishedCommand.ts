import { BookId } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type MoveToPublishedCommand = Command<
  'MoveToPublishedCommand',
  {
    bookId: BookId;
  }
>;
