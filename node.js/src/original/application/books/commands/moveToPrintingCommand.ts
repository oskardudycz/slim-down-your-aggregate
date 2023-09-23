import { BookId } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type MoveToPrintingCommand = Command<
  'MoveToPrintingCommand',
  {
    bookId: BookId;
  }
>;
