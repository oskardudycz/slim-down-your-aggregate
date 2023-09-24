import { Book } from '../book';
import { BookId } from '../entities';

export interface IBooksRepository {
  findById(bookId: BookId): Promise<Book | null>;

  store(book: Book): Promise<void>;
}
