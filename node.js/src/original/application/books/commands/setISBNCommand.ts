//import { BookId } from '#original';
import { ISBN } from 'src/slimmed/domain/books/entities';
import { BookId } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type SetISBNCommand = Command<
  'SetISBNCommand',
  {
    bookId: BookId;
    isbn: ISBN;
  }
>;
