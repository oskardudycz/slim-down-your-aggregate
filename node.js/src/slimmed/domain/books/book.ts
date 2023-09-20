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
  Publisher,
  Reviewer,
  Title,
  Translation,
  chapterNumber,
} from './entities';
import { BookId } from './entities/bookId';
import { IPublishingHouse } from './services/publishingHouse';
import { IBookFactory } from './factories/bookFactory';
import { InvalidOperationError, InvalidStateError } from '#core/errors';
import {
  Approved,
  BookEvent,
  ChapterAdded,
  DraftCreated,
  FormatAdded,
  FormatRemoved,
  ISBNSet,
  MovedToEditing,
  MovedToOutOfPrint,
  MovedToPrinting,
  Published,
  ReviewerAdded,
  TranslationAdded,
} from './bookEvent';

export class Initial extends Aggregate<BookId, BookEvent> {
  constructor(id: BookId) {
    super(id);
  }

  createDraft(
    title: Title,
    author: Author,
    publisher: Publisher,
    edition: PositiveNumber,
    genre: Genre | null,
  ): void {
    const event: DraftCreated = {
      type: 'DraftCreated',
      data: {
        bookId: this.id,
        title,
        author,
        publisher,
        edition,
        genre,
      },
    };
    this.addDomainEvent(event);
  }
}

export class Draft extends Aggregate<BookId, BookEvent> {
  #genre: Genre | null;
  #chapters: Chapter[];
  public readonly status = State.Writing;

  constructor(id: BookId, genre: Genre | null, chapters: Chapter[] = []) {
    super(id);
    this.#genre = genre;
    this.#chapters = chapters;
  }

  addChapter(title: ChapterTitle, content: ChapterContent): void {
    if (this.#chapters.some((chap) => chap.title === title)) {
      throw InvalidStateError(`Chapter with title ${title} already exists.`);
    }

    if (
      this.#chapters.length > 0 &&
      !this.#chapters[this.#chapters.length - 1].title.startsWith(
        `Chapter ${this.#chapters.length}`,
      )
    ) {
      throw InvalidStateError(
        `Chapter should be added in sequence. The title of the next chapter should be 'Chapter ${
          this.#chapters.length + 1
        }'`,
      );
    }

    const chapter = new Chapter(
      chapterNumber(this.#chapters.length + 1),
      title,
      content,
    );
    this.#chapters.push(chapter);

    const event: ChapterAdded = {
      type: 'ChapterAdded',
      data: {
        bookId: this.id,
        chapter,
      },
    };

    this.addDomainEvent(event);
  }

  moveToEditing(): void {
    if (this.#chapters.length < 1) {
      throw InvalidStateError(
        'A book must have at least one chapter to move to the Editing state.',
      );
    }

    if (this.#genre === null) {
      throw InvalidStateError(
        'Book can be moved to editing only when genre is specified',
      );
    }

    const event: MovedToEditing = {
      type: 'MovedToEditing',
      data: {
        bookId: this.id,
      },
    };

    this.addDomainEvent(event);
  }
}

export class UnderEditing extends Aggregate<BookId, BookEvent> {
  #isbn: ISBN | null;
  #genre: Genre;
  #committeeApproval: CommitteeApproval | null;
  #reviewers: Reviewer[];
  #translations: Translation[];
  #formats: Format[];
  #chapters: Chapter[];
  public readonly status = State.Editing;

  constructor(
    id: BookId,
    genre: Genre,
    chapters: Chapter[],
    reviewers: Reviewer[] = [],
    translations: Translation[] = [],
    formats: Format[] = [],
    isbn?: ISBN | null,
    committeeApproval?: CommitteeApproval | null,
  ) {
    super(id);
    this.#genre = genre;
    this.#chapters = chapters;
    this.#reviewers = reviewers;
    this.#translations = translations;
    this.#formats = formats;
    this.#isbn = isbn ?? null;
    this.#committeeApproval = committeeApproval ?? null;
  }

  addTranslation(translation: Translation): void {
    if (this.#translations.length >= 5) {
      throw InvalidStateError(
        'Cannot add more translations. Maximum 5 translations are allowed.',
      );
    }

    this.#translations.push(translation);

    const event: TranslationAdded = {
      type: 'TranslationAdded',
      data: {
        bookId: this.id,
        translation,
      },
    };

    this.addDomainEvent(event);
  }

  addFormat(format: Format): void {
    if (this.#formats.some((f) => f.formatType === format.formatType)) {
      throw InvalidStateError(`Format ${format.formatType} already exists.`);
    }

    this.#formats.push(format);

    const event: FormatAdded = {
      type: 'FormatAdded',
      data: {
        bookId: this.id,
        format,
      },
    };

    this.addDomainEvent(event);
  }

  removeFormat(format: Format): void {
    const existingFormat = this.#formats.find(
      (f) => f.formatType === format.formatType,
    );
    if (!existingFormat) {
      throw InvalidStateError(`Format ${format.formatType} does not exist.`);
    }

    this.#formats = this.#formats.filter(
      (f) => f.formatType !== format.formatType,
    );

    const event: FormatRemoved = {
      type: 'FormatRemoved',
      data: {
        bookId: this.id,
        format,
      },
    };

    this.addDomainEvent(event);
  }

  addReviewer(reviewer: Reviewer): void {
    if (this.#reviewers.some((r) => r.name === reviewer.name)) {
      const reviewerName: string = reviewer.name;
      throw InvalidStateError(`${reviewerName} is already a reviewer.`);
    }

    this.#reviewers.push(reviewer);

    const event: ReviewerAdded = {
      type: 'ReviewerAdded',
      data: {
        bookId: this.id,
        reviewer,
      },
    };

    this.addDomainEvent(event);
  }

  approve(committeeApproval: CommitteeApproval): void {
    if (this.#reviewers.length < 3) {
      throw InvalidStateError(
        'A book cannot be approved unless it has been reviewed by at least three reviewers.',
      );
    }

    this.#committeeApproval = committeeApproval;

    const event: Approved = {
      type: 'Approved',
      data: {
        bookId: this.id,
        committeeApproval,
      },
    };

    this.addDomainEvent(event);
  }

  setISBN(isbn: ISBN): void {
    if (this.#isbn !== null) {
      throw InvalidStateError('Cannot change already set ISBN.');
    }

    this.#isbn = isbn;

    const event: ISBNSet = {
      type: 'ISBNSet',
      data: {
        bookId: this.id,
        isbn,
      },
    };

    this.addDomainEvent(event);
  }

  moveToPrinting(publishingHouse: IPublishingHouse): void {
    if (this.#chapters.length < 1) {
      throw InvalidStateError(
        'A book must have at least one chapter to move to the printing state.',
      );
    }

    if (this.#committeeApproval === null) {
      throw InvalidStateError(
        'Cannot move to printing state until the book has been approved.',
      );
    }

    if (this.#reviewers.length < 3) {
      throw InvalidStateError(
        'A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.',
      );
    }

    if (this.#genre === null) {
      throw InvalidStateError(
        'Book can be moved to the printing only when genre is specified',
      );
    }

    // Check for genre limit
    if (!publishingHouse.isGenreLimitReached(this.#genre)) {
      throw InvalidStateError(
        'Cannot move to printing until the genre limit is reached.',
      );
    }

    const event: MovedToPrinting = {
      type: 'MovedToPrinting',
      data: {
        bookId: this.id,
      },
    };

    this.addDomainEvent(event);
  }
}

export class InPrint extends Aggregate<BookId, BookEvent> {
  #title: Title;
  #author: Author;
  #isbn: ISBN | null;
  #reviewers: Reviewer[];
  public readonly status = State.Printing;

  constructor(
    id: BookId,
    title: Title,
    author: Author,
    isbn?: ISBN | null,
    reviewers?: Reviewer[] | null,
  ) {
    super(id);
    this.#title = title;
    this.#author = author;
    this.#isbn = isbn ?? null;
    this.#reviewers = reviewers ?? [];
  }

  moveToPublished(): void {
    if (this.#isbn === null) {
      throw InvalidStateError('Cannot move to Published state without ISBN.');
    }

    if (this.#reviewers.length < 3) {
      throw InvalidStateError(
        'A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.',
      );
    }

    const event: Published = {
      type: 'Published',
      data: {
        bookId: this.id,
        isbn: this.#isbn,
        title: this.#title,
        author: this.#author,
      },
    };

    this.addDomainEvent(event);
  }
}

export class PublishedBook extends Aggregate<BookId, BookEvent> {
  #formats: Format[];
  public readonly status = State.Published;

  constructor(id: BookId, formats: Format[]) {
    super(id);
    this.#formats = formats;
  }

  moveToOutOfPrint(): void {
    const totalCopies = this.#formats.reduce(
      (acc, format) => acc + format.totalCopies,
      0,
    );
    const totalSoldCopies = this.#formats.reduce(
      (acc, format) => acc + format.soldCopies,
      0,
    );

    if (totalSoldCopies / totalCopies > 0.1) {
      throw InvalidStateError(
        'Cannot move to Out of Print state if more than 10% of total copies are unsold.',
      );
    }

    const event: MovedToOutOfPrint = {
      type: 'MovedToOutOfPrint',
      data: {
        bookId: this.id,
      },
    };

    this.addDomainEvent(event);
  }
}

export class OutOfPrint extends Aggregate<BookId, BookEvent> {
  constructor(id: BookId) {
    super(id);
  }
}

export type Book =
  | Initial
  | Draft
  | UnderEditing
  | InPrint
  | PublishedBook
  | OutOfPrint;

export class BookFactory implements IBookFactory {
  create(
    bookId: BookId,
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
        return new Draft(bookId, genre, chapters);
      case State.Editing: {
        if (genre == null)
          throw InvalidOperationError('Genre should be provided!');

        return new UnderEditing(
          bookId,
          genre,
          chapters,
          reviewers,
          translations,
          formats,
          isbn,
          committeeApproval,
        );
      }
      case State.Printing:
        return new InPrint(bookId, title, author, isbn, reviewers);
      case State.Published:
        return new PublishedBook(bookId, formats);
      case State.OutOfPrint:
        return new OutOfPrint(bookId);
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
