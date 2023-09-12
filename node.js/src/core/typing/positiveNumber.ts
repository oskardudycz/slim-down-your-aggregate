import { assertPositiveNumber } from '#core/validation';
import { Brand } from '.';

export type PositiveNumber<T extends string = string> = Brand<
  number,
  'PositiveNumber'
> & { readonly __positiveNumberType: T };

export const positiveNumber = <S extends string, T extends string = string>(
  arg: S extends 0 ? never : S,
): PositiveNumber<T> => {
  return assertPositiveNumber(arg) as PositiveNumber<T>;
};
