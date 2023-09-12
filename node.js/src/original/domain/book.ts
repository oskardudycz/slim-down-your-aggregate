import { Aggregate } from '../infrastructure/aggregates';
import { BookId } from './entities/bookId';

export class Book extends Aggregate<BookId> {}
