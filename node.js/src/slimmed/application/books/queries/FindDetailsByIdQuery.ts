//import { BookId } from '#original';
import { BookId } from '../../../domain/books/entities';
import { Query } from '../../../infrastructure/queries';

export type FindDetailsByIdQuery = Query<
  'FindDetailsByIdQuery',
  {
    bookId: BookId;
  }
>;
