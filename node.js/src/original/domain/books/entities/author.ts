import { NonEmptyString } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type Author = DeepReadonly<{
  authorId: AuthorId;
}>;

export type AuthorId = NonEmptyString<'AuthorId'>;

export type AuthorFirstName = NonEmptyString<'AuthorFirstName'>;

export type AuthorLastName = NonEmptyString<'AuthorLastName'>;
