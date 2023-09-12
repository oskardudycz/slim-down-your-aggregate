import { assertPositiveNumber } from '#core/validation';
import { Brand } from '.';

export type PositiveNumber<T extends string = string> = T &
  Brand<number, 'PositiveNumber'> & { readonly __positiveNumberType: T };

export type NonEmptyStringCheck<T extends number> = T extends 0 ? never : T;

export const positiveNumber = <S extends number, T extends string = string>(
  arg: NonEmptyStringCheck<S>,
): PositiveNumber<T> => {
  return assertPositiveNumber(arg) as PositiveNumber<T>;
};
