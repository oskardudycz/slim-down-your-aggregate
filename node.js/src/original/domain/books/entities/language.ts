import { DeepReadonly, NonEmptyString, NonEmptyUuid } from '#core/typing';

export type LanguageId = NonEmptyUuid<'LanguageId'>;
export type LanguageName = NonEmptyString<'LanguageName'>;

export type Language = DeepReadonly<{ Id: LanguageId; Name: LanguageName }>;
