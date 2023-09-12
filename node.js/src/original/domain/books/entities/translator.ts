import { NonEmptyString, NonEmptyUuid } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type TranslatorId = NonEmptyUuid<'TranslatorId'>;
export type TranslatorName = NonEmptyString<'TranslatorName'>;

export type Translator = DeepReadonly<{
  id: TranslatorId;
  name: TranslatorName;
}>;
