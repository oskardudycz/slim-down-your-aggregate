import { LanguageEntity } from '../../languages';
import { TranslatorEntity } from '../../translators';

export type TranslationVO = {
  language: LanguageEntity;
  translator: TranslatorEntity;
};
