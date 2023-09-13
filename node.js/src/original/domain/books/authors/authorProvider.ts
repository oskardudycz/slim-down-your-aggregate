import { Author } from '../entities';
import { AuthorIdOrData } from './authorIdOrData';

export interface IAuthorProvider {
  getOrCreate(authorIdOrData: AuthorIdOrData): Promise<Author>;
}
