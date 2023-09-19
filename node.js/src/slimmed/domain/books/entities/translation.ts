import { DeepReadonly } from 'ts-essentials';
import { Language } from './language';
import { Translator } from './translator';

export type Translation = DeepReadonly<{
  language: Language;
  translator: Translator;
}>;
