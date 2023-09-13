import { Flavour } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type Query<
  QueryType extends string = string,
  QueryData extends Record<string, unknown> = Record<string, unknown>,
> = Flavour<
  DeepReadonly<{
    type: QueryType;
    data: QueryData;
  }>,
  'Query'
>;
