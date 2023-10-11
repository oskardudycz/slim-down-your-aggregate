import { InvalidStateError } from '#core/errors';
import { PositiveNumber } from '#core/typing';
import { ChapterAdded, Draft, DraftCreated, Initial } from '.';
import {
  Title,
  Author,
  Publisher,
  Genre,
  Chapter,
  chapterNumber,
  ChapterTitle,
  ChapterContent,
} from '../entities';
import { MovedToEditing } from '../underEditing';

export const createDraft = (
  _initial: Initial,
  title: Title,
  author: Author,
  publisher: Publisher,
  edition: PositiveNumber,
  genre: Genre | null,
): DraftCreated => {
  return {
    type: 'DraftCreated',
    data: {
      title,
      author,
      publisher,
      edition,
      genre,
    },
  };
};

export const addChapter = (
  book: Draft,
  title: ChapterTitle,
  content: ChapterContent,
): ChapterAdded => {
  if (book.chapterTitles.includes(title)) {
    throw InvalidStateError(`Chapter with title ${title} already exists.`);
  }

  if (!title.startsWith(`Chapter ${book.chapterTitles.length + 1}`)) {
    throw InvalidStateError(
      `Chapter should be added in sequence. The title of the next chapter should be 'Chapter ${
        book.chapterTitles.length + 1
      }'`,
    );
  }

  const chapter = new Chapter(
    chapterNumber(book.chapterTitles.length + 1),
    title,
    content,
  );

  return {
    type: 'ChapterAdded',
    data: {
      chapter,
    },
  };
};

export const moveToEditing = (book: Draft): MovedToEditing => {
  if (book.chapterTitles.length < 1) {
    throw InvalidStateError(
      'A book must have at least one chapter to move to the Editing state.',
    );
  }

  if (!book.genre) {
    throw InvalidStateError(
      'Book can be moved to editing only when genre is specified',
    );
  }

  return {
    type: 'MovedToEditing',
    data: {
      genre: book.genre,
    },
  };
};
