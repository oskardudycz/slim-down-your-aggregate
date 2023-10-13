import { LanguageEntity } from '../../languages';
import { TranslatorEntity } from '../../translators';

export type TranslationEntity = {
  bookId: string;
  language: LanguageEntity;
  translator: TranslatorEntity;
};
