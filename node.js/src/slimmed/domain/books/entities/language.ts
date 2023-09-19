import { NonEmptyString, NonEmptyUuid } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type LanguageId = NonEmptyUuid<'LanguageId'>;
export type LanguageName = NonEmptyString<'LanguageName'>;

export type Language = DeepReadonly<{ id: LanguageId; name: LanguageName }>;
