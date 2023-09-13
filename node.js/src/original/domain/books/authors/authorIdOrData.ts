import { AuthorFirstName, AuthorId, AuthorLastName } from '../entities';

export type AuthorIdOrData =
  | AuthorId
  | { firstName: AuthorFirstName; lastName: AuthorLastName };
