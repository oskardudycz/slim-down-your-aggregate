import { PositiveNumber } from '#core/typing';
import { AuthorIdOrData } from 'src/original/domain/books/authors';
import {
  BookId,
  Genre,
  PublisherId,
  Title,
} from 'src/original/domain/books/entities';
import { Command } from 'src/original/infrastructure/commands';

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
