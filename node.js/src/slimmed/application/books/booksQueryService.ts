import { BookDetails } from '../../persistence/books/dtos';
import { FindDetailsByIdQuery } from './queries';
import { IBooksQueryRepository } from '../../persistence/books/repositories';

export interface IBooksQueryService {
  findDetailsById(query: FindDetailsByIdQuery): Promise<BookDetails | null>;
}

export class BooksQueryService implements IBooksQueryService {
  constructor(private repository: IBooksQueryRepository) {}

  findDetailsById(query: FindDetailsByIdQuery): Promise<BookDetails | null> {
    return this.repository.findDetailsById(query.data.bookId);
  }
}
