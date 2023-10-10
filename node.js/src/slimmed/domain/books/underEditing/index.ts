import { InvalidStateError } from '#core/errors';
import { PositiveNumber } from '#core/typing';
import { DomainEvent } from '../../../infrastructure/events';
import {
  BookId,
  Genre,
  ReviewerId,
  FormatType,
  Translation,
  Reviewer,
  CommitteeApproval,
  ISBN,
  Format,
} from '../entities';
import { LanguageId } from '../entities/language';
import { IPublishingHouse } from '../services';
import { MovedToPrinting } from '../inPrint';

export class UnderEditing {
  public get id() {
    return this._id;
  }
  constructor(
    private readonly _id: BookId,
    private readonly genre: Genre | null,
    private readonly isISBNSet: boolean,
    private readonly isApproved: boolean,
    private readonly reviewers: ReviewerId[],
    private readonly translationLanguages: LanguageId[],
    private readonly formatTypes: FormatType[],
  ) {}

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

  public static evolve(
    book: UnderEditing,
    event: UnderEditingEvent,
  ): UnderEditing {
    const { type, data } = event;

    switch (type) {
      case 'MovedToEditing': {
        const { bookId, genre } = data;

        return new UnderEditing(bookId, genre, false, false, [], [], []);
      }
      case 'TranslationAdded': {
        const {
          bookId,
          translation: {
            language: { id: languageId },
          },
        } = data;

        return new UnderEditing(
          bookId,
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          [...book.translationLanguages, languageId],
          book.formatTypes,
        );
      }
      case 'TranslationRemoved': {
        const {
          bookId,
          translation: {
            language: { id: languageId },
          },
        } = data;

        return new UnderEditing(
          bookId,
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          book.translationLanguages.filter((t) => t != languageId),
          book.formatTypes,
        );
      }
      case 'FormatAdded': {
        const {
          bookId,
          format: { formatType },
        } = data;

        return new UnderEditing(
          bookId,
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          book.translationLanguages,
          [...book.formatTypes, formatType],
        );
      }
      case 'FormatRemoved': {
        const {
          bookId,
          format: { formatType },
        } = data;

        return new UnderEditing(
          bookId,
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          book.translationLanguages,
          book.formatTypes.filter((t) => t != formatType),
        );
      }
      case 'ReviewerAdded': {
        const {
          bookId,
          reviewer: { id: reviewerId },
        } = data;

        return new UnderEditing(
          bookId,
          book.genre,
          book.isISBNSet,
          book.isApproved,
          [...book.reviewers, reviewerId],
          book.translationLanguages,
          book.formatTypes,
        );
      }
      case 'Approved': {
        const { bookId } = data;

        return new UnderEditing(
          bookId,
          book.genre,
          book.isISBNSet,
          true,
          book.reviewers,
          book.translationLanguages,
          book.formatTypes,
        );
      }
      case 'ISBNSet': {
        const { bookId } = data;

        return new UnderEditing(
          bookId,
          book.genre,
          true,
          book.isApproved,
          book.reviewers,
          book.translationLanguages,
          book.formatTypes,
        );
      }
    }
  }
}

export type FormatAdded = DomainEvent<
  'FormatAdded',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type FormatRemoved = DomainEvent<
  'FormatRemoved',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type TranslationAdded = DomainEvent<
  'TranslationAdded',
  {
    bookId: BookId;
    translation: Translation;
  }
>;

export type TranslationRemoved = DomainEvent<
  'TranslationRemoved',
  {
    bookId: BookId;
    translation: Translation;
  }
>;

export type ReviewerAdded = DomainEvent<
  'ReviewerAdded',
  {
    bookId: BookId;
    reviewer: Reviewer;
  }
>;

export type MovedToEditing = DomainEvent<
  'MovedToEditing',
  {
    bookId: BookId;
    genre: Genre | null;
  }
>;

export type Approved = DomainEvent<
  'Approved',
  {
    bookId: BookId;
    committeeApproval: CommitteeApproval;
  }
>;

export type ISBNSet = DomainEvent<
  'ISBNSet',
  {
    bookId: BookId;
    isbn: ISBN;
  }
>;

export type UnderEditingEvent =
  | MovedToEditing
  | TranslationAdded
  | TranslationRemoved
  | FormatAdded
  | FormatRemoved
  | ReviewerAdded
  | Approved
  | ISBNSet;
