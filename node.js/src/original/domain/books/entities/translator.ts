import { DeepReadonly, NonEmptyString, NonEmptyUuid } from '#core/typing';

export type TranslatorId = NonEmptyUuid<'TranslatorId'>;
export type TranslatorName = NonEmptyString<'TranslatorName'>;

export type Translator = DeepReadonly<{
  Id: TranslatorId;
  Name: TranslatorName;
}>;
