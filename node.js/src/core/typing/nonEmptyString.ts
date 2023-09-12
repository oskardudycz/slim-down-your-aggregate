import { assertNotEmptyString } from '#core/validation';
import { Brand } from '.';

export type NonEmptyString<T extends string = string> = T &
  Brand<string, 'NonEmptyString'> & { readonly __nonEmptyStringType: T };

export type NonEmptyStringCheck<T extends string> = T extends '' ? never : T;

export const nonEmptyString = <S extends string, T extends string = string>(
  arg: NonEmptyStringCheck<S>,
): NonEmptyString<T> => {
  return assertNotEmptyString(arg) as NonEmptyString<T>;
};
