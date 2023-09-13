import { Book } from '../book';
import { BookId } from '../entities';

export interface IBooksRepository {
  findById(bookId: BookId): Promise<Book | null>;

  add(book: Book): Promise<void>;

  update(book: Book): Promise<void>;
}
