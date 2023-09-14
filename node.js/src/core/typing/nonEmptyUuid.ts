import { assertNotEmptyString } from '#core/validation';
import { Brand } from '.';

export type NonEmptyUuid<T extends string = string> = T &
  Brand<string, 'NonEmptyUuid'> & { readonly __nonEmptyStringType: T };

export const nonEmptyUuid = <S extends string, T extends string = string>(
  arg: S extends '' ? never : S,
): NonEmptyUuid<T> => {
  return parseNonEmptyUuid(arg);
};

export const parseNonEmptyUuid = <T extends string = string>(
  arg: string | undefined,
): NonEmptyUuid<T> => {
  //TODO: Add uuid validation
  return assertNotEmptyString(arg) as NonEmptyUuid<T>;
};
