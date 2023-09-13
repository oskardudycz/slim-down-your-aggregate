import { BookDetails } from '../dtos';
import { BookId } from '../entities';

export interface IBooksQueryRepository {
  findDetailsById(bookId: BookId): Promise<BookDetails>;
}
