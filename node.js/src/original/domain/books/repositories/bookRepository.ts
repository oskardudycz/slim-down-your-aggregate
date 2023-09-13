import { Book } from '../book';

export interface IBooksRepository {
  findById(bookId: Book): Promise<Book | null>;

  add(book: Book): Promise<void>;

  update(book: Book): Promise<void>;
}
