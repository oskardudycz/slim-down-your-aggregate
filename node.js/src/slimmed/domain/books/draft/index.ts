import { InvalidStateError } from '#core/errors';
import { PositiveNumber } from '#core/typing';
import { DomainEvent } from '../../../infrastructure/events';
import {
  BookId,
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

export class Draft {
  public get id() {
    return this._id;
  }
  constructor(
    private readonly _id: BookId,
    private readonly genre: Genre | null,
    private readonly chapterTitles: ChapterTitle[] = [],
  ) {}

  addChapter(title: ChapterTitle, content: ChapterContent): ChapterAdded {
    if (this.chapterTitles.includes(title)) {
      throw InvalidStateError(`Chapter with title ${title} already exists.`);
    }

    if (!title.startsWith(`Chapter ${this.chapterTitles.length + 1}`)) {
      throw InvalidStateError(
        `Chapter should be added in sequence. The title of the next chapter should be 'Chapter ${
          this.chapterTitles.length + 1
        }'`,
      );
    }

    const chapter = new Chapter(
      chapterNumber(this.chapterTitles.length + 1),
      title,
      content,
    );

    return {
      type: 'ChapterAdded',
      data: {
        bookId: this.id,
        chapter,
      },
    };
  }

  moveToEditing(): MovedToEditing {
    if (this.chapterTitles.length < 1) {
      throw InvalidStateError(
        'A book must have at least one chapter to move to the Editing state.',
      );
    }

    if (!this.genre) {
      throw InvalidStateError(
        'Book can be moved to editing only when genre is specified',
      );
    }

    return {
      type: 'MovedToEditing',
      data: {
        bookId: this.id,
        genre: this.genre,
      },
    };
  }

  public static evolve(book: Draft, event: DraftEvent): Draft {
    const { type, data } = event;

    switch (type) {
      case 'DraftCreated': {
        const { bookId, genre } = data;

        return new Draft(bookId, genre, []);
      }
      case 'ChapterAdded': {
        const { bookId, chapter } = data;

        return new Draft(bookId, book.genre, [
          ...book.chapterTitles,
          chapter.title,
        ]);
      }
    }
  }
}

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

export type DraftEvent = DraftCreated | ChapterAdded;
