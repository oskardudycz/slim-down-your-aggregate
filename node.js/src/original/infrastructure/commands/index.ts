import { Flavour } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type Command<
  CommandType extends string = string,
  CommandData extends Record<string, unknown> = Record<string, unknown>,
> = Flavour<
  DeepReadonly<{
    type: CommandType;
    data: CommandData;
  }>,
  'Command'
>;
