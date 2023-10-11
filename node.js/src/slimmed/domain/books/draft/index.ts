import { InvalidStateError } from '#core/errors';
import { PositiveNumber } from '#core/typing';
import { DomainEvent } from '../../../infrastructure/events';
import {
  Genre,
  ChapterTitle,
  ChapterContent,
  Chapter,
  chapterNumber,
  Author,
  Publisher,
  Title,
} from '../entities';
import { MovedToEditing } from '../underEditing';

export class Initial {
  public static readonly initial = new Initial();
}

export class Draft {
  constructor(
    private readonly genre: Genre | null,
    private readonly chapterTitles: ChapterTitle[] = [],
  ) {}

  static createDraft(
    _initial: Initial,
    title: Title,
    author: Author,
    publisher: Publisher,
    edition: PositiveNumber,
    genre: Genre | null,
  ): DraftCreated {
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
  }

  static addChapter(
    book: Draft,
    title: ChapterTitle,
    content: ChapterContent,
  ): ChapterAdded {
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
  }

  static moveToEditing(book: Draft): MovedToEditing {
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
  }

  public static evolve(book: Draft, event: DraftEvent): Draft {
    const { type, data } = event;

    switch (type) {
      case 'DraftCreated': {
        const { genre } = data;

        return new Draft(genre, []);
      }
      case 'ChapterAdded': {
        const { chapter } = data;

        return new Draft(book.genre, [...book.chapterTitles, chapter.title]);
      }
    }
  }

  public static readonly initial = new Draft(null, []);
}

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

export type DraftEvent = DraftCreated | ChapterAdded;
