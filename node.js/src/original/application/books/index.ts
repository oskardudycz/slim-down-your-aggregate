import {
  BookQueryRepository,
  BooksRepository,
} from 'src/original/persistence/books/repositories';
import { BooksService } from './booksService';
import { AuthorProvider } from 'src/original/persistence/authors';
import { PublisherProvider } from 'src/original/persistence/publishers';
import { BooksQueryService } from './booksQueryService';
import { orm } from 'src/original/persistence/orm';
import { Book } from 'src/original/domain/books/book';

export const configureBooks = () => {
  return {
    service: new BooksService(
      new BooksRepository(orm, new Book.BookFactory()),
      new AuthorProvider(orm),
      new PublisherProvider(orm),
    ),
    queryService: new BooksQueryService(new BookQueryRepository(orm)),
  };
};
