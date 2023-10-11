import { InvalidStateError } from '#core/errors';
import { PositiveNumber, positiveNumber } from '#core/typing';
import { DomainEvent } from '../../../infrastructure/events';
import {
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
  constructor(
    private readonly genre: Genre | null,
    private readonly isISBNSet: boolean,
    private readonly isApproved: boolean,
    private readonly reviewers: ReviewerId[],
    private readonly translationLanguages: LanguageId[],
    private readonly formats: {
      formatType: FormatType;
      totalCopies: PositiveNumber;
    }[],
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
        translation,
      },
    };
  }

  addFormat(format: Format): FormatAdded {
    const { formatType } = format;

    if (this.formats.some((f) => f.formatType == formatType)) {
      throw InvalidStateError(`Format ${format.formatType} already exists.`);
    }

    return {
      type: 'FormatAdded',
      data: {
        format,
      },
    };
  }

  removeFormat(format: Format): FormatRemoved {
    const { formatType } = format;

    if (!this.formats.some((f) => f.formatType == formatType)) {
      throw InvalidStateError(`Format ${format.formatType} does not exist.`);
    }

    return {
      type: 'FormatRemoved',
      data: {
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

    const totalCopies = this.formats.reduce(
      (acc, format) => acc + format.totalCopies,
      0,
    );

    return {
      type: 'MovedToPrinting',
      data: {
        totalCopies: positiveNumber(totalCopies),
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
        const { genre } = data;

        return new UnderEditing(genre, false, false, [], [], []);
      }
      case 'TranslationAdded': {
        const {
          translation: {
            language: { id: languageId },
          },
        } = data;

        return new UnderEditing(
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          [...book.translationLanguages, languageId],
          book.formats,
        );
      }
      case 'TranslationRemoved': {
        const {
          translation: {
            language: { id: languageId },
          },
        } = data;

        return new UnderEditing(
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          book.translationLanguages.filter((t) => t != languageId),
          book.formats,
        );
      }
      case 'FormatAdded': {
        const {
          format: { formatType, totalCopies },
        } = data;

        return new UnderEditing(
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          book.translationLanguages,
          [...book.formats, { formatType, totalCopies }],
        );
      }
      case 'FormatRemoved': {
        const {
          format: { formatType },
        } = data;

        return new UnderEditing(
          book.genre,
          book.isISBNSet,
          book.isApproved,
          book.reviewers,
          book.translationLanguages,
          book.formats.filter((f) => f.formatType != formatType),
        );
      }
      case 'ReviewerAdded': {
        const {
          reviewer: { id: reviewerId },
        } = data;

        return new UnderEditing(
          book.genre,
          book.isISBNSet,
          book.isApproved,
          [...book.reviewers, reviewerId],
          book.translationLanguages,
          book.formats,
        );
      }
      case 'Approved': {
        return new UnderEditing(
          book.genre,
          book.isISBNSet,
          true,
          book.reviewers,
          book.translationLanguages,
          book.formats,
        );
      }
      case 'ISBNSet': {
        return new UnderEditing(
          book.genre,
          true,
          book.isApproved,
          book.reviewers,
          book.translationLanguages,
          book.formats,
        );
      }
    }
  }

  public static readonly default = new UnderEditing(
    null,
    false,
    false,
    [],
    [],
    [],
  );
}

export type FormatAdded = DomainEvent<
  'FormatAdded',
  {
    format: Format;
  }
>;

export type FormatRemoved = DomainEvent<
  'FormatRemoved',
  {
    format: Format;
  }
>;

export type TranslationAdded = DomainEvent<
  'TranslationAdded',
  {
    translation: Translation;
  }
>;

export type TranslationRemoved = DomainEvent<
  'TranslationRemoved',
  {
    translation: Translation;
  }
>;

export type ReviewerAdded = DomainEvent<
  'ReviewerAdded',
  {
    reviewer: Reviewer;
  }
>;

export type MovedToEditing = DomainEvent<
  'MovedToEditing',
  {
    genre: Genre | null;
  }
>;

export type Approved = DomainEvent<
  'Approved',
  {
    committeeApproval: CommitteeApproval;
  }
>;

export type ISBNSet = DomainEvent<
  'ISBNSet',
  {
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
