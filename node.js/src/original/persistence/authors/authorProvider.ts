import {
  AuthorIdOrData,
  IAuthorProvider,
} from 'src/original/domain/books/authors';
import { Author } from 'src/original/domain/books/entities';

export class AuthorProvider implements IAuthorProvider {
  getOrCreate(_authorIdOrData: AuthorIdOrData): Promise<Author> {
    throw new Error('Method not implemented.');
  }
}
