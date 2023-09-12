import { DeepReadonly } from '#core/typing';
import { Language } from './language';
import { Translator } from './translator';

export type Translation = DeepReadonly<{
  language: Language;
  translator: Translator;
}>;
