import { PositiveNumber } from '#core/typing';
import { DomainEvent } from '../../infrastructure/events';
import {
  Author,
  AuthorId,
  BookId,
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
    bookId: BookId;
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
    bookId: BookId;
    chapter: Chapter;
  }
>;

export type FormatAdded = DomainEvent<
  'FormatAdded',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type FormatRemoved = DomainEvent<
  'FormatRemoved',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type TranslationAdded = DomainEvent<
  'TranslationAdded',
  {
    bookId: BookId;
    translation: Translation;
  }
>;

export type TranslationRemoved = DomainEvent<
  'TranslationRemoved',
  {
    bookId: BookId;
    translation: Translation;
  }
>;

export type ReviewerAdded = DomainEvent<
  'ReviewerAdded',
  {
    bookId: BookId;
    reviewer: Reviewer;
  }
>;

export type MovedToEditing = DomainEvent<
  'MovedToEditing',
  {
    bookId: BookId;
    genre: Genre | null;
  }
>;

export type Approved = DomainEvent<
  'Approved',
  {
    bookId: BookId;
    committeeApproval: CommitteeApproval;
  }
>;

export type ISBNSet = DomainEvent<
  'ISBNSet',
  {
    bookId: BookId;
    isbn: ISBN;
  }
>;

export type MovedToPrinting = DomainEvent<
  'MovedToPrinting',
  {
    bookId: BookId;
  }
>;

export type Published = DomainEvent<
  'Published',
  {
    bookId: BookId;
  }
>;

export type MovedToOutOfPrint = DomainEvent<
  'MovedToOutOfPrint',
  {
    bookId: BookId;
  }
>;

export type DraftEvent = DraftCreated | ChapterAdded;

export type UnderEditingEvent =
  | MovedToEditing
  | TranslationAdded
  | TranslationRemoved
  | FormatAdded
  | FormatRemoved
  | ReviewerAdded
  | Approved
  | ISBNSet;

export type InPrintEvent = MovedToPrinting;

export type PublishedEvent = Published;

export type OutOfPrintEvent = MovedToOutOfPrint;

export type BookEvent =
  | DraftEvent
  | UnderEditingEvent
  | InPrintEvent
  | PublishedEvent
  | OutOfPrintEvent;

export type PublishedExternal = DomainEvent<
  'Published',
  {
    bookId: BookId;
    isbn: ISBN;
    title: Title;
    authorId: AuthorId;
  }
>;

export type BookExternalEvent =
  | DraftEvent
  | UnderEditingEvent
  | PublishedExternal
  | OutOfPrintEvent;
