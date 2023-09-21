import { PositiveNumber } from '#core/typing';
import { Aggregate } from '../../infrastructure/aggregates';
import {
  Author,
  Chapter,
  ChapterContent,
  ChapterTitle,
  CommitteeApproval,
  Format,
  Genre,
  ISBN,
  Reviewer,
  Title,
  Translation,
} from './entities';
import { IBookFactory } from './factories/bookFactory';
import { InvalidOperationError } from '#core/errors';
import { BookEvent } from './bookEvent';
import { DeepReadonly } from 'ts-essentials';

export type Initial = DeepReadonly<{
  status: State.Initial;
}>;

export type Draft = DeepReadonly<{
  status: State.Writing;
  genre: Genre | null;
  chapters: Chapter[];
}>;

export type UnderEditing = DeepReadonly<{
  status: State.Editing;
  isbn: ISBN | null;
  genre: Genre;
  committeeApproval: CommitteeApproval | null;
  reviewers: Reviewer[];
  translations: Translation[];
  formats: Format[];
  chapters: Chapter[];
}>;

export type InPrint = DeepReadonly<{
  status: State.Printing;
  title: Title;
  author: Author;
  isbn: ISBN | null;
  reviewers: Reviewer[];
}>;

export type PublishedBook = DeepReadonly<{
  status: State.Published;
  formats: Format[];
}>;

export type OutOfPrint = DeepReadonly<{
  status: State.OutOfPrint;
  formats: Format[];
}>;

export const evolve = (book: Book, event: BookEvent): Book => {
  const { type, data } = event;

  switch (type) {
    case 'MovedToEditing': {
      if (book.status !== State.Writing) return book;

      return {
        status: State.Editing,
        chapters: book.chapters,

        genre: book.genre,
      };
    }
    default:
      return book;
  }
  return {} as unknown as Book;
};

export type Book =
  | Initial
  | Draft
  | UnderEditing
  | InPrint
  | PublishedBook
  | OutOfPrint;

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
        return { status: State.Writing, genre, chapters };
      case State.Editing: {
        if (genre == null)
          throw InvalidOperationError('Genre should be provided!');

        return {
          status: State.Editing,
          genre,
          chapters,
          reviewers,
          translations,
          formats,
          isbn,
          committeeApproval,
        };
      }
      case State.Printing:
        return {
          status: State.Printing,
          title,
          author,
          isbn,
          reviewers,
        };
      case State.Published:
        return {
          status: State.Published,
          formats,
        };
      case State.OutOfPrint:
        return {
          status: State.OutOfPrint,
        };
      case State.Initial:
        return {
          status: State.Initial,
        };
    }
  }
}

export enum State {
  Initial = 'Initial',
  Writing = 'Writing',
  Editing = 'Editing',
  Printing = 'Printing',
  Published = 'Published',
  OutOfPrint = 'OutOfPrint',
}
