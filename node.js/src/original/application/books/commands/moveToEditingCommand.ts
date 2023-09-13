import { BookId } from 'src/original/domain/books/entities';
import { Command } from 'src/original/infrastructure/commands';

export type MoveToEditingCommand = Command<
  'MoveToEditingCommand',
  {
    bookId: BookId;
  }
>;
