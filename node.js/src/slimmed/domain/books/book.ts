import { positiveNumber } from '#core/typing';
import {
  Author,
  AuthorId,
  Chapter,
  CommitteeApproval,
  Format,
  Genre,
  ISBN,
  Reviewer,
  Title,
  Translation,
} from './entities';
import { BookId } from './entities/bookId';
import { IBookFactory } from './factories/bookFactory';
import { InvalidOperationError } from '#core/errors';
import {
  Initial,
  Draft,
  DraftEvent,
  isInitial,
  initialDraft,
  isDraft,
  evolve as evolveDraft,
} from './draft';
import {
  UnderEditing,
  UnderEditingEvent,
  evolve as evolveUnderEditing,
  initial as initialUnderEditing,
  isUnderEditing,
} from './underEditing';
import {
  InPrint,
  InPrintEvent,
  evolve as evolveInPrint,
  initial as initialInPrint,
  isInPrint,
} from './inPrint';
import {
  PublishedBook,
  PublishedEvent,
  evolve as evolvePublished,
  initial as initialPublished,
  isPublished,
} from './published';
import {
  OutOfPrint,
  OutOfPrintEvent,
  evolve as evolveOutOfPrint,
  initial as initialOutOfPrint,
} from './outOfPrint';
import { DomainEvent } from 'src/original/infrastructure/events';

export type Book =
  | Initial
  | Draft
  | UnderEditing
  | PublishedBook
  | InPrint
  | OutOfPrint;

export const evolve = (book: Book, event: BookEvent): Book => {
  switch (event.type) {
    case 'DraftCreated': {
      if (!isInitial(book) || !isDraft(book)) return book;
      return evolveDraft(initialDraft, event);
    }
    case 'ChapterAdded': {
      if (!isDraft(book)) return book;

      return evolve(book, event);
    }
    case 'MovedToEditing': {
      if (!isDraft(book)) return book;

      return evolveUnderEditing(initialUnderEditing, event);
    }
    case 'TranslationAdded':
    case 'TranslationRemoved':
    case 'FormatAdded':
    case 'FormatRemoved':
    case 'ReviewerAdded':
    case 'Approved':
    case 'ISBNSet': {
      if (!isUnderEditing(book)) return book;

      return evolveUnderEditing(book, event);
    }
    case 'MovedToPrinting': {
      if (!isUnderEditing(book)) return book;

      return evolveInPrint(initialInPrint, event);
    }
    case 'Published': {
      if (!isInPrint(book)) return book;

      return evolvePublished(initialPublished, event);
    }
    case 'MovedToOutOfPrint': {
      if (!isPublished(book)) return book;

      return evolveOutOfPrint(initialOutOfPrint, event);
    }

    default: {
      return book;
    }
  }
};

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

export class BookFactory implements IBookFactory {
  create(
    state: State,
    title: Title,
    author: Author,
    genre: Genre | null,
    isbn: ISBN | null,
    committeeApproval: CommitteeApproval | null,
    reviewers: Reviewer[],
    chapters: Chapter[],
    translations: Translation[],
    formats: Format[],
  ): Book {
    switch (state) {
      case State.Writing:
        return {
          status: 'Draft',
          genre,
          chapterTitles: chapters.map((ch) => ch.title),
        };
      case State.Editing: {
        if (genre == null)
          throw InvalidOperationError('Genre should be provided!');

        return {
          status: 'UnderEditing',
          genre,
          isISBNSet: !!isbn,
          isApproved: !!committeeApproval,
          reviewers: reviewers.map((r) => r.id),
          translationLanguages: translations.map((r) => r.language.id),
          formats: formats.map((f) => {
            return { formatType: f.formatType, totalCopies: f.totalCopies };
          }),
        };
      }
      case State.Printing: {
        const totalCopies = formats.reduce(
          (acc, format) => acc + format.totalCopies,
          0,
        );
        return { status: 'InPrint', totalCopies: positiveNumber(totalCopies) };
      }
      case State.Published: {
        const totalCopies = formats.reduce(
          (acc, format) => acc + format.totalCopies,
          0,
        );
        const totalSoldCopies = formats.reduce(
          (acc, format) => acc + format.soldCopies,
          0,
        );

        return {
          status: 'Published',
          totalCopies: positiveNumber(totalCopies),
          totalSoldCopies: positiveNumber(totalSoldCopies),
        };
      }
      case State.OutOfPrint:
        return { status: 'OutOfPrint' };
    }
  }
}

export enum State {
  Writing = 'Writing',
  Editing = 'Editing',
  Printing = 'Printing',
  Published = 'Published',
  OutOfPrint = 'OutOfPrint',
}
