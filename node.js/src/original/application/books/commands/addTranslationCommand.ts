//import { BookId } from '#original';
import { BookId, Translation } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type AddTranslationCommand = Command<
  'AddTranslationCommand',
  {
    bookId: BookId;
    translation: Translation;
  }
>;
