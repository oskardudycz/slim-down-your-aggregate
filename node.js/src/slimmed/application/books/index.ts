import { v4 as uuid } from 'uuid';
import {
  BookQueryRepository,
  BooksRepository,
} from '../../persistence/books/repositories';
import { BooksService } from './booksService';
import { AuthorProvider } from '../../persistence/authors';
import { PublisherProvider } from '../../persistence/publishers';
import { BooksQueryService } from './booksQueryService';
import { Book } from '../../domain/books/book';
import {
  PublishingHouseOrm,
  publishingHouseOrm,
} from '../../persistence/publishingHouseOrm';

export const EXISTING_PUBLISHER_ID = uuid();
export const EXISTING_PUBLISHER_NAME = uuid();

const seedPublishingHouse = (orm: PublishingHouseOrm) => {
  orm.publishers.add(EXISTING_PUBLISHER_ID, {
    id: EXISTING_PUBLISHER_ID,
    name: EXISTING_PUBLISHER_NAME,
  });
};

export const configureBooks = () => {
  const orm = publishingHouseOrm(seedPublishingHouse);
  return {
    service: new BooksService(
      new BooksRepository(orm, new Book.BookFactory()),
      new AuthorProvider(orm),
      new PublisherProvider(orm),
    ),
    queryService: new BooksQueryService(new BookQueryRepository(orm)),
  };
};
