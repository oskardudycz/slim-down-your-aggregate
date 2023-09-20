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
import { InvalidStateError } from '#core/errors';
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

export class Book extends Aggregate<BookId, BookEvent> {
  #currentState: State;
  #title: Title;
  #author: Author;
  #genre: Genre | null;
  #isbn: ISBN | null;
  #committeeApproval: CommitteeApproval | null;
  #reviewers: Reviewer[];
  #chapters: Chapter[];
  #translations: Translation[];
  #formats: Format[];

  static createDraft(
    bookId: BookId,
    title: Title,
    author: Author,
    publishingHouse: IPublishingHouse,
    publisher: Publisher,
    edition: PositiveNumber,
    genre: Genre | null,
  ): Book {
    const book = new Book(bookId, State.Writing, title, author, genre);

    const event: DraftCreated = {
      type: 'DraftCreated',
      data: {
        bookId,
        title,
        author,
        publisher,
        edition,
        genre,
      },
    };
    book.addDomainEvent(event);

    return book;
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
    if (this.#currentState !== State.Writing) {
      throw InvalidStateError(
        'Cannot move to Editing state from the current state.',
      );
    }

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

    this.#currentState = State.Editing;

    const event: MovedToEditing = {
      type: 'MovedToEditing',
      data: {
        bookId: this.id,
      },
    };

    this.addDomainEvent(event);
  }

  addTranslation(translation: Translation): void {
    if (this.#currentState !== State.Editing) {
      throw InvalidStateError(
        'Cannot add translation of a book that is not in the Editing state.',
      );
    }

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
    if (this.#currentState !== State.Editing) {
      throw InvalidStateError(
        'Cannot add format of a book that is not in the Editing state.',
      );
    }

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
    if (this.#currentState !== State.Editing) {
      throw InvalidStateError(
        'Cannot remove format of a book that is not in the Editing state.',
      );
    }

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
    if (this.#currentState !== State.Editing) {
      throw InvalidStateError(
        'Cannot approve a book that is not in the Editing state.',
      );
    }

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
    if (this.#currentState !== State.Editing) {
      throw InvalidStateError(
        'Cannot approve a book that is not in the Editing state.',
      );
    }

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
    if (this.#currentState !== State.Editing) {
      throw InvalidStateError(
        'Cannot approve a book that is not in the Editing state.',
      );
    }

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
    if (this.#currentState !== State.Editing) {
      throw InvalidStateError(
        'Cannot move to printing from the current state.',
      );
    }

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

    this.#currentState = State.Printing;

    const event: MovedToPrinting = {
      type: 'MovedToPrinting',
      data: {
        bookId: this.id,
      },
    };

    this.addDomainEvent(event);
  }

  moveToPublished(): void {
    if (
      this.#currentState !== State.Printing ||
      this.#translations.length < 5
    ) {
      throw InvalidStateError(
        'Cannot move to Published state from the current state.',
      );
    }

    if (this.#isbn === null) {
      throw InvalidStateError('Cannot move to Published state without ISBN.');
    }

    if (this.#reviewers.length < 3) {
      throw InvalidStateError(
        'A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.',
      );
    }

    this.#currentState = State.Published;

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

  moveToOutOfPrint(): void {
    if (this.#currentState !== State.Published) {
      throw InvalidStateError(
        'Cannot move to Out of Print state from the current state.',
      );
    }

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

    this.#currentState = State.OutOfPrint;

    const event: MovedToOutOfPrint = {
      type: 'MovedToOutOfPrint',
      data: {
        bookId: this.id,
      },
    };

    this.addDomainEvent(event);
  }

  private constructor(
    id: BookId,
    currentState: State,
    title: Title,
    author: Author,
    genre: Genre | null,
    isbn?: ISBN | null,
    committeeApproval?: CommitteeApproval | null,
    reviewers?: Reviewer[] | null,
    chapters?: Chapter[] | null,
    translations?: Translation[] | null,
    formats?: Format[] | null,
  ) {
    super(id);
    this.#currentState = currentState;
    this.#title = title;
    this.#author = author;
    this.#genre = genre;
    this.#isbn = isbn ?? null;
    this.#committeeApproval = committeeApproval ?? null;
    this.#reviewers = reviewers ?? [];
    this.#chapters = chapters ?? [];
    this.#translations = translations ?? [];
    this.#formats = formats ?? [];
  }

  static BookFactory = class implements IBookFactory {
    create(
      bookId: BookId,
      state: State,
      title: Title,
      author: Author,
      genre: Genre | null,
      isbn: ISBN | null,
      committeeApproval: CommitteeApproval | null,
      reviewers: Reviewer[] | null,
      chapters: Chapter[] | null,
      translations: Translation[] | null,
      formats: Format[] | null,
    ): Book {
      return new Book(
        bookId,
        state,
        title,
        author,
        genre,
        isbn,
        committeeApproval,
        reviewers,
        chapters,
        translations,
        formats,
      );
    }
  };
}

export enum State {
  Writing = 'Writing',
  Editing = 'Editing',
  Printing = 'Printing',
  Published = 'Published',
  OutOfPrint = 'OutOfPrint',
}
