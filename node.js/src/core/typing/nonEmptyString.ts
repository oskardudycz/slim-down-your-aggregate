import { assertNotEmptyString } from '#core/validation';
import { Brand } from '.';

export type NonEmptyString<T extends string = string> = Brand<
  string,
  'NonEmptyString'
> & { readonly __nonEmptyStringType: T };

export const nonEmptyString = <S extends string, T extends string = string>(
  arg: (S extends '' ? never : S) | undefined,
): NonEmptyString<T> => {
  return parseNonEmptyString(arg);
};

export const parseNonEmptyString = <T extends string = string>(
  arg: string | undefined,
): NonEmptyString<T> => {
  return assertNotEmptyString(arg) as NonEmptyString<T>;
};
