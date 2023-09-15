import { Book } from 'src/original/domain/books/book';
import { BookId } from 'src/original/domain/books/entities';
import { IBooksRepository } from 'src/original/domain/books/repositories';
import { ORM } from '../../orm';
import { IBookFactory } from 'src/original/domain/books/factories';
import { bookMapper } from '../../mappers/bookMapper';

export class BooksRepository implements IBooksRepository {
  constructor(
    private orm: ORM,
    private bookFactory: IBookFactory,
  ) {}

  async findById(bookId: BookId): Promise<Book | null> {
    const entity = await this.orm.books.get(bookId);

    if (entity === null) return null;

    return bookMapper.mapFromEntity(entity, this.bookFactory);
  }

  add(book: Book): Promise<void> {
    return this.orm.books.store(book.id, bookMapper.mapToEntity(book));
  }

  update(book: Book): Promise<void> {
    return this.orm.books.store(book.id, bookMapper.mapToEntity(book));
  }
}
