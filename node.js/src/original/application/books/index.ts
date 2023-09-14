import {
  BookQueryRepository,
  BooksRepository,
} from 'src/original/persistence/books/repositories';
import { BooksService } from './booksService';
import { AuthorProvider } from 'src/original/persistence/authors';
import { PublisherProvider } from 'src/original/persistence/publishers';
import { BooksQueryService } from './booksQueryService';

export const configureBooks = () => {
  return {
    service: new BooksService(
      new BooksRepository(),
      new AuthorProvider(),
      new PublisherProvider(),
    ),
    queryService: new BooksQueryService(new BookQueryRepository()),
  };
};
