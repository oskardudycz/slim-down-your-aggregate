import { BookId } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type MoveToOutOfPrintCommand = Command<
  'MoveToOutOfPrintCommand',
  {
    bookId: BookId;
  }
>;
