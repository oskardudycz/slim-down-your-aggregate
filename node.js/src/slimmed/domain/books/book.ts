import { PositiveNumber, positiveNumber } from '#core/typing';
import {
  Author,
  Chapter,
  ChapterContent,
  ChapterTitle,
  CommitteeApproval,
  Format,
  FormatType,
  Genre,
  ISBN,
  Publisher,
  Reviewer,
  ReviewerId,
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
import { LanguageId } from './entities/language';
import { Ratio, ratio } from '#core/typing/ratio';

export abstract class Book {
  public get id() {
    return this._id;
  }
  constructor(protected _id: BookId) {}

  public static evolve(book: Book, event: BookEvent) {
    const { type, data } = event;

    switch (type) {
      case 'DraftCreated': {
        if (!(book instanceof Initial)) return book;

        const { bookId, genre } = data;

        return new Draft(bookId, genre, []);
      }
      case 'MovedToEditing': {
        if (!(book instanceof Draft)) return book;

        const { bookId, genre } = data;

        return new UnderEditing(bookId, genre, false, false, [], [], []);
      }
      case 'MovedToPrinting': {
        if (!(book instanceof UnderEditing)) return book;

        const { bookId } = data;

        // TODO: Add methods to set total items per format
        return new InPrint(bookId);
      }
      case 'Published': {
        if (!(book instanceof InPrint)) return book;

        const { bookId } = data;

        // TODO: Add methods to set sold copies
        return new PublishedBook(bookId, positiveNumber(1), positiveNumber(1));
      }
      case 'MovedToOutOfPrint': {
        if (!(book instanceof PublishedBook)) return book;

        const { bookId } = data;

        return new OutOfPrint(bookId);
      }

      default: {
        return book;
      }
    }
  }
}
export class Initial extends Book {
  constructor(id: BookId) {
    super(id);
  }

  createDraft(
    title: Title,
    author: Author,
    publisher: Publisher,
    edition: PositiveNumber,
    genre: Genre | null,
  ): DraftCreated {
    return {
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
  }
}

export class Draft extends Book {
  constructor(
    id: BookId,
    private genre: Genre | null,
    private chapterTitles: ChapterTitle[] = [],
  ) {
    super(id);
  }

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

    this.chapterTitles.push(title);

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
}

export class UnderEditing extends Book {
  constructor(
    id: BookId,
    private genre: Genre | null,
    private isISBNSet: boolean,
    private isApproved: boolean,
    private reviewers: ReviewerId[],
    private translationLanguages: LanguageId[],
    private formatTypes: FormatType[],
  ) {
    super(id);
  }

  addTranslation(
    translation: Translation,
    maximumNumberOfTranslations: PositiveNumber,
  ): TranslationAdded {
    const { language } = translation;

    if (this.translationLanguages.includes(language.id))
      throw InvalidStateError(
        `Translation to ${language.name} already exists.`,
      );

    if (this.translationLanguages.length > maximumNumberOfTranslations) {
      throw InvalidStateError(
        `Cannot add more translations. Maximum ${maximumNumberOfTranslations} translations are allowed.`,
      );
    }

    this.translationLanguages.push(language.id);

    return {
      type: 'TranslationAdded',
      data: {
        bookId: this.id,
        translation,
      },
    };
  }

  addFormat(format: Format): FormatAdded {
    const { formatType } = format;

    if (this.formatTypes.includes(formatType)) {
      throw InvalidStateError(`Format ${format.formatType} already exists.`);
    }

    this.formatTypes.push(formatType);

    return {
      type: 'FormatAdded',
      data: {
        bookId: this.id,
        format,
      },
    };
  }

  removeFormat(format: Format): FormatRemoved {
    const { formatType } = format;

    if (!this.formatTypes.includes(formatType)) {
      throw InvalidStateError(`Format ${format.formatType} does not exist.`);
    }

    this.formatTypes = this.formatTypes.filter((f) => f !== format.formatType);

    return {
      type: 'FormatRemoved',
      data: {
        bookId: this.id,
        format,
      },
    };
  }

  addReviewer(reviewer: Reviewer): ReviewerAdded {
    const { id: reviewerId } = reviewer;

    if (this.reviewers.includes(reviewerId)) {
      const reviewerName: string = reviewer.name;
      throw InvalidStateError(`${reviewerName} is already a reviewer.`);
    }

    this.reviewers.push(reviewerId);

    return {
      type: 'ReviewerAdded',
      data: {
        bookId: this.id,
        reviewer,
      },
    };
  }

  approve(
    committeeApproval: CommitteeApproval,
    minimumReviewersRequiredForApproval: PositiveNumber,
  ): Approved {
    if (this.reviewers.length < minimumReviewersRequiredForApproval) {
      throw InvalidStateError(
        'A book cannot be approved unless it has been reviewed by at least three reviewers.',
      );
    }

    this.isApproved = true;

    return {
      type: 'Approved',
      data: {
        bookId: this.id,
        committeeApproval,
      },
    };
  }

  setISBN(isbn: ISBN): ISBNSet {
    if (this.isISBNSet) {
      throw InvalidStateError('Cannot change already set ISBN.');
    }

    this.isISBNSet = true;

    return {
      type: 'ISBNSet',
      data: {
        bookId: this.id,
        isbn,
      },
    };
  }

  moveToPrinting(publishingHouse: IPublishingHouse): MovedToPrinting {
    if (this.isApproved === null) {
      throw InvalidStateError(
        'Cannot move to printing state until the book has been approved.',
      );
    }

    if (this.genre === null) {
      throw InvalidStateError(
        'Book can be moved to the printing only when genre is specified',
      );
    }

    // Check for genre limit
    if (!publishingHouse.isGenreLimitReached(this.genre)) {
      throw InvalidStateError(
        'Cannot move to printing until the genre limit is reached.',
      );
    }

    return {
      type: 'MovedToPrinting',
      data: {
        bookId: this.id,
      },
    };
  }
}

export class InPrint extends Book {
  constructor(id: BookId) {
    super(id);
  }

  moveToPublished(): Published {
    return {
      type: 'Published',
      data: {
        bookId: this.id,
      },
    };
  }
}

export class PublishedBook extends Book {
  public readonly status = State.Published;

  constructor(
    id: BookId,
    private readonly totalCopies: PositiveNumber,
    private readonly totalSoldCopies: PositiveNumber,
  ) {
    super(id);
  }

  private get unsoldCopiesRatio(): Ratio {
    return ratio((this.totalCopies - this.totalSoldCopies) / this.totalCopies);
  }

  moveToOutOfPrint(
    maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio,
  ): MovedToOutOfPrint {
    if (this.unsoldCopiesRatio > maxAllowedUnsoldCopiesRatioToGoOutOfPrint) {
      throw InvalidStateError(
        'Cannot move to Out of Print state if more than 10% of total copies are unsold.',
      );
    }

    return {
      type: 'MovedToOutOfPrint',
      data: {
        bookId: this.id,
      },
    };
  }
}

export class OutOfPrint extends Book {
  constructor(id: BookId) {
    super(id);
  }
}

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
        return new Draft(
          bookId,
          genre,
          chapters.map((ch) => ch.title),
        );
      case State.Editing: {
        if (genre == null)
          throw InvalidOperationError('Genre should be provided!');

        return new UnderEditing(
          bookId,
          genre,
          !!isbn,
          !!committeeApproval,
          reviewers.map((r) => r.id),
          translations.map((r) => r.language.id),
          formats.map((f) => f.formatType),
        );
      }
      case State.Printing: {
        return new InPrint(bookId);
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
          bookId,
          positiveNumber(totalCopies),
          positiveNumber(totalSoldCopies),
        );
      }
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
