import { BookDetails } from 'src/original/domain/books/dtos';
import { BookId } from 'src/original/domain/books/entities';

export interface IBookQueryService {
  findDetailsById(bookId: BookId): Promise<BookDetails | null>;
}

export class BooksQueryService implements IBookQueryService {
  constructor(private repository: IBookQueryService) {}

  findDetailsById(bookId: BookId): Promise<BookDetails | null> {
    return this.repository.findDetailsById(bookId);
  }
}
