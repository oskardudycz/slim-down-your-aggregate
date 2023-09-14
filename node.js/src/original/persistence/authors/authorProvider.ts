import { v4 as uuid } from 'uuid';
import {
  AuthorIdOrData,
  IAuthorProvider,
} from 'src/original/domain/books/authors';
import {
  Author,
  AuthorFirstName,
  AuthorId,
} from 'src/original/domain/books/entities';
import { ORM } from '../orm';
import { nonEmptyString } from '#core/typing';

export class AuthorProvider implements IAuthorProvider {
  constructor(private orm: ORM) {}

  async getOrCreate(authorIdOrData: AuthorIdOrData): Promise<Author> {
    if ('firstName' in authorIdOrData) {
      const authorId: AuthorId = nonEmptyString(uuid());
      const firstName: AuthorFirstName = nonEmptyString(
        authorIdOrData.firstName,
      );
      const lastName: AuthorFirstName = nonEmptyString(
        authorIdOrData.firstName,
      );
      await this.orm.authors.store(authorId, {
        id: authorId,
        firstName,
        lastName,
      });

      return { authorId, firstName, lastName };
    } else {
      const entity = await this.orm.authors.get(authorIdOrData);

      if (!entity) throw Error(`Author with id ${authorIdOrData} not found`);

      const { id, firstName, lastName } = entity;

      return {
        authorId: nonEmptyString(id),
        firstName: nonEmptyString(firstName),
        lastName: nonEmptyString(lastName),
      };
    }
  }
}
