import { PositiveNumber, positiveNumber } from '#core/typing';
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
import { Draft, DraftEvent } from './draft';
import { InPrint, InPrintEvent } from './inPrint';
import { Initial } from './initial';
import { OutOfPrint, OutOfPrintEvent } from './outOfPrint';
import { PublishedBook, PublishedEvent } from './published';
import { UnderEditing, UnderEditingEvent } from './underEditing';
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
      if (!(book instanceof Initial)) return book;
      return Draft.evolve(new Draft(null, []), event);
    }
    case 'ChapterAdded': {
      if (!(book instanceof Draft)) return book;

      return Draft.evolve(book, event);
    }
    case 'MovedToEditing': {
      if (!(book instanceof Draft)) return book;

      return UnderEditing.evolve(
        new UnderEditing(null, false, false, [], [], []),
        event,
      );
    }
    case 'TranslationAdded':
    case 'TranslationRemoved':
    case 'FormatAdded':
    case 'FormatRemoved':
    case 'ReviewerAdded':
    case 'Approved':
    case 'ISBNSet': {
      if (!(book instanceof UnderEditing)) return book;

      return UnderEditing.evolve(book, event);
    }
    case 'MovedToPrinting': {
      if (!(book instanceof UnderEditing)) return book;

      return InPrint.evolve(new InPrint(), event);
    }
    case 'Published': {
      if (!(book instanceof InPrint)) return book;

      return PublishedBook.evolve(
        new PublishedBook(0 as PositiveNumber, 0 as PositiveNumber),
        event,
      );
    }
    case 'MovedToOutOfPrint': {
      if (!(book instanceof PublishedBook)) return book;

      return OutOfPrint.evolve(new OutOfPrint(), event);
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
        return new Draft(
          genre,
          chapters.map((ch) => ch.title),
        );
      case State.Editing: {
        if (genre == null)
          throw InvalidOperationError('Genre should be provided!');

        return new UnderEditing(
          genre,
          !!isbn,
          !!committeeApproval,
          reviewers.map((r) => r.id),
          translations.map((r) => r.language.id),
          formats.map((f) => f.formatType),
        );
      }
      case State.Printing: {
        return new InPrint();
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

        return new PublishedBook(
          positiveNumber(totalCopies),
          positiveNumber(totalSoldCopies),
        );
      }
      case State.OutOfPrint:
        return new OutOfPrint();
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
