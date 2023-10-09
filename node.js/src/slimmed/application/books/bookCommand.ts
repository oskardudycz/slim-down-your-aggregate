import { PositiveNumber } from '#core/typing';
import { AuthorIdOrData } from '../../domain/books/authors';
import {
  BookId,
  ChapterContent,
  ChapterTitle,
  CommitteeApproval,
  Format,
  Genre,
  ISBN,
  PublisherId,
  Reviewer,
  Title,
  Translation,
} from '../../domain/books/entities';
import { Command } from '../../infrastructure/commands';

export type CreateDraft = Command<
  'CreateDraftCommand',
  {
    bookId: BookId;
    title: Title;
    author: AuthorIdOrData;
    publisherId: PublisherId;
    edition: PositiveNumber;
    genre: Genre | null;
  }
>;

export type AddChapter = Command<
  'AddChapterCommand',
  {
    bookId: BookId;
    chapterTitle: ChapterTitle;
    chapterContent: ChapterContent;
  }
>;

export type MoveToEditing = Command<
  'MoveToEditingCommand',
  {
    bookId: BookId;
  }
>;

export type AddTranslation = Command<
  'AddTranslationCommand',
  {
    bookId: BookId;
    translation: Translation;
  }
>;

export type AddFormat = Command<
  'AddFormatCommand',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type RemoveFormat = Command<
  'RemoveFormatCommand',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type AddReviewer = Command<
  'AddReviewerCommand',
  {
    bookId: BookId;
    reviewer: Reviewer;
  }
>;

export type Approve = Command<
  'ApproveCommand',
  {
    bookId: BookId;
    committeeApproval: CommitteeApproval;
  }
>;

export type SetISBN = Command<
  'SetISBNCommand',
  {
    bookId: BookId;
    isbn: ISBN;
  }
>;

export type MoveToPrinting = Command<
  'MoveToPrintingCommand',
  {
    bookId: BookId;
  }
>;

export type MoveToPublished = Command<
  'MoveToPublishedCommand',
  {
    bookId: BookId;
  }
>;

export type MoveToOutOfPrint = Command<
  'MoveToOutOfPrintCommand',
  {
    bookId: BookId;
  }
>;

export type BookCommand =
  | CreateDraft
  | AddChapter
  | MoveToEditing
  | AddTranslation
  | AddFormat
  | RemoveFormat
  | AddReviewer
  | Approve
  | SetISBN
  | MoveToPrinting
  | MoveToPublished
  | MoveToOutOfPrint;
