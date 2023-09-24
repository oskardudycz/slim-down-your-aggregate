import { ValidationError } from '#core/errors';
import { assertPositiveNumber } from '#core/validation';
import { Brand } from '.';

export type Ratio<T extends string = string> = Brand<number, 'Ratio'> & {
  readonly _ratioType: T;
};

export const ratio = <S extends number, T extends string = string>(
  arg: S extends 0 ? never : S,
): Ratio<T> => {
  return parseRatio(arg);
};

export const parseRatio = <T extends string = string>(
  arg: string | number | undefined,
): Ratio<T> => {
  const positiveNumber = assertPositiveNumber(arg);

  if (positiveNumber > 1)
    throw ValidationError('Ratio needs to be between 0 and 1');

  return positiveNumber as Ratio<T>;
};
