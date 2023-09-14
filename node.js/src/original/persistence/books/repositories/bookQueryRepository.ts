import { BookDetails } from 'src/original/domain/books/dtos';
import { BookId } from 'src/original/domain/books/entities';
import { IBooksQueryRepository } from 'src/original/domain/books/repositories';

export class BookQueryRepository implements IBooksQueryRepository {
  findDetailsById(_bookId: BookId): Promise<BookDetails> {
    throw new Error('Method not implemented.');
  }
}
