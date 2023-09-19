import { BookId } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type MoveToEditingCommand = Command<
  'MoveToEditingCommand',
  {
    bookId: BookId;
  }
>;
