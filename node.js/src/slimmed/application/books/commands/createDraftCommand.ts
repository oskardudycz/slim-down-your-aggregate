import { PositiveNumber } from '#core/typing';
import { AuthorIdOrData } from '../../../domain/books/authors';
import {
  BookId,
  Genre,
  PublisherId,
  Title,
} from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type CreateDraftCommand = Command<
  'CreateDraftCommand',
  {
    bookId: BookId;
    title: Title;
    author: AuthorIdOrData;
    publisherId: PublisherId;
    edition: PositiveNumber;
    genre: Genre | null;
  }
>;
