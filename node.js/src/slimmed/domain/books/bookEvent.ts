import { PositiveNumber } from '#core/typing';
import { DomainEvent } from '../../infrastructure/events';
import {
  Author,
  Chapter,
  CommitteeApproval,
  Format,
  Genre,
  ISBN,
  Publisher,
  Reviewer,
  Title,
  Translation,
} from './entities';

export type DraftCreated = DomainEvent<
  'DraftCreated',
  {
    title: Title;
    author: Author;
    publisher: Publisher;
    edition: PositiveNumber;
    genre: Genre | null;
  }
>;

export type ChapterAdded = DomainEvent<
  'ChapterAdded',
  {
    chapter: Chapter;
  }
>;

export type FormatAdded = DomainEvent<
  'FormatAdded',
  {
    format: Format;
  }
>;

export type FormatRemoved = DomainEvent<
  'FormatRemoved',
  {
    format: Format;
  }
>;

export type TranslationAdded = DomainEvent<
  'TranslationAdded',
  {
    translation: Translation;
  }
>;

export type TranslationRemoved = DomainEvent<
  'TranslationRemoved',
  {
    translation: Translation;
  }
>;

export type ReviewerAdded = DomainEvent<
  'ReviewerAdded',
  {
    reviewer: Reviewer;
  }
>;

export type MovedToEditing = DomainEvent<'MovedToEditing', {}>;

export type Approved = DomainEvent<
  'Approved',
  {
    committeeApproval: CommitteeApproval;
  }
>;

export type ISBNSet = DomainEvent<
  'ISBNSet',
  {
    isbn: ISBN;
  }
>;

export type MovedToPrinting = DomainEvent<'MovedToPrinting', {}>;

export type Published = DomainEvent<
  'Published',
  {
    isbn: ISBN;
    title: Title;
    author: Author;
  }
>;

export type MovedToOutOfPrint = DomainEvent<'MovedToOutOfPrint', {}>;

export type DraftEvent = ChapterAdded | MovedToEditing;

export type BookEvent =
  | DraftCreated
  | DraftEvent
  | FormatAdded
  | FormatRemoved
  | TranslationAdded
  | TranslationRemoved
  | ReviewerAdded
  | Approved
  | ISBNSet
  | MovedToPrinting
  | Published
  | MovedToOutOfPrint;
