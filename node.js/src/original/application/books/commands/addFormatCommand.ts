//import { BookId } from '#original';
import { BookId, Format } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type AddFormatCommand = Command<
  'AddFormatCommand',
  {
    bookId: BookId;
    format: Format;
  }
>;
