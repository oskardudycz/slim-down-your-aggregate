import { Book } from '../book';
import { BookEvent } from '../bookEvent';
import { BookId } from '../entities';

export interface IBooksRepository {
  findById(bookId: BookId): Promise<Book | null>;

  store(bookId: BookId, events: BookEvent[]): Promise<void>;
}
