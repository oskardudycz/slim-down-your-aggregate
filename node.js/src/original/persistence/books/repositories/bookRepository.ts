import { Book } from 'src/original/domain/books/book';
import { BookId } from 'src/original/domain/books/entities';
import { IBooksRepository } from 'src/original/domain/books/repositories';
import { ORM } from '../../orm';

export class BooksRepository implements IBooksRepository {
  constructor(private orm: ORM) {}

  findById(_bookId: BookId): Promise<Book | null> {
    throw new Error('Method not implemented.');
  }

  add(_book: Book): Promise<void> {
    throw new Error('Method not implemented.');
  }

  update(_book: Book): Promise<void> {
    throw new Error('Method not implemented.');
  }
}
