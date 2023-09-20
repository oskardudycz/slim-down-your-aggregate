import { AuthorEntity } from './authors';
import { BookEntity } from './books';
import {
  ChapterEntity,
  FormatEntity,
  TranslationEntity,
} from './books/entities';
import { BookReviewerEntity } from './books/entities/bookReviewerEntity';
import { OutboxMessageEntity } from './core/outbox/outboxMessageEntity';
import { Database, EntitiesCollection, getDatabase } from './orm';
import { PublisherEntity } from './publishers';

export interface PublishingHouseOrm extends Database {
  authors: EntitiesCollection<AuthorEntity>;
  books: EntitiesCollection<BookEntity>;
  bookChapters: EntitiesCollection<ChapterEntity>;
  bookFormats: EntitiesCollection<FormatEntity>;
  bookReviewers: EntitiesCollection<BookReviewerEntity>;
  bookTranslations: EntitiesCollection<TranslationEntity>;
  publishers: EntitiesCollection<PublisherEntity>;
  outbox: EntitiesCollection<OutboxMessageEntity>;
}

export const publishingHouseOrm = (
  seed?: (orm: PublishingHouseOrm) => void,
): PublishingHouseOrm => {
  const database = getDatabase();

  const orm = {
    ...database,
    authors: database.table<AuthorEntity>('authors'),
    books: database.table<BookEntity>('books'),
    bookChapters: database.table<ChapterEntity>('bookChapters'),
    bookFormats: database.table<FormatEntity>('bookFormats'),
    bookReviewers: database.table<BookReviewerEntity>('bookReviewers'),
    bookTranslations: database.table<TranslationEntity>('bookTranslations'),
    publishers: database.table<PublisherEntity>('publishers'),
    outbox: database.table<OutboxMessageEntity>('outbox'),
  };

  if (seed) {
    seed(orm);
  }

  return orm;
};
