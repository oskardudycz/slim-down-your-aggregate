//import { BookId } from '#original';
import { BookId } from 'src/original/domain/books/entities';
import { Query } from 'src/original/infrastructure/queries';

export type FindDetailsByIdQuery = Query<
  'FindDetailsByIdQuery',
  {
    bookId: BookId;
  }
>;
