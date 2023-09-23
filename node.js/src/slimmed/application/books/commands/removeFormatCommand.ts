//import { BookId } from '#original';
import { BookId, Format } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type RemoveFormatCommand = Command<
  'RemoveFormatCommand',
  {
    bookId: BookId;
    format: Format;
  }
>;
