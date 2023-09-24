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
import {
  InvalidOperationError,
  InvalidStateError,
  ValidationError,
} from '#core/errors';
import {
  Approved,
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
    private isGenreSet: boolean,
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

    if (!this.isGenreSet) {
      throw InvalidStateError(
        'Book can be moved to editing only when genre is specified',
      );
    }

    return {
      type: 'MovedToEditing',
      data: {
        bookId: this.id,
      },
    };
  }
}

export class UnderEditing extends Book {
  constructor(
    id: BookId,
    private genre: Genre,
    private isISBNSet: boolean,
    private isApproved: boolean,
    private chapterCount: PositiveNumber,
    private reviewers: ReviewerId[],
    private readonly minimumReviewersRequiredForApproval: PositiveNumber,
    private translationLanguages: LanguageId[],
    private readonly maximumNumberOfTranslations: PositiveNumber,
    private formatTypes: FormatType[],
  ) {
    super(id);
  }

  addTranslation(translation: Translation): TranslationAdded {
    const { language } = translation;

    if (this.translationLanguages.includes(language.id))
      throw InvalidStateError(
        `Translation to ${language.name} already exists.`,
      );

    if (this.translationLanguages.length > this.maximumNumberOfTranslations) {
      throw InvalidStateError(
        `Cannot add more translations. Maximum ${this.maximumNumberOfTranslations} translations are allowed.`,
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

  approve(committeeApproval: CommitteeApproval): Approved {
    if (this.reviewers.length < this.minimumReviewersRequiredForApproval) {
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
    if (this.chapterCount < 1) {
      throw InvalidStateError(
        'A book must have at least one chapter to move to the printing state.',
      );
    }

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
  constructor(
    id: BookId,
    private readonly title: Title,
    private readonly author: Author,
    private readonly isbn: ISBN,
  ) {
    super(id);
  }

  moveToPublished(): Published {
    return {
      type: 'Published',
      data: {
        bookId: this.id,
        isbn: this.isbn,
        title: this.title,
        author: this.author,
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
    private readonly maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio,
  ) {
    super(id);
  }

  private get unsoldCopiesRatio(): Ratio {
    return ratio((this.totalCopies - this.totalSoldCopies) / this.totalCopies);
  }

  moveToOutOfPrint(): MovedToOutOfPrint {
    if (
      this.unsoldCopiesRatio > this.maxAllowedUnsoldCopiesRatioToGoOutOfPrint
    ) {
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
          !!genre,
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
          positiveNumber(chapters.length),
          reviewers.map((r) => r.id),
          positiveNumber(3),
          translations.map((r) => r.language.id),
          positiveNumber(5),
          formats.map((f) => f.formatType),
        );
      }
      case State.Printing: {
        if (!isbn) throw ValidationError('ISBN needs to be set');

        return new InPrint(bookId, title, author, isbn);
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
          ratio(0.1),
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
