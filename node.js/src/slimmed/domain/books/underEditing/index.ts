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

  static addTranslation(
    state: UnderEditing,
    translation: Translation,
    maximumNumberOfTranslations: PositiveNumber,
  ): TranslationAdded {
    const { language } = translation;

    if (state.translationLanguages.includes(language.id))
      throw InvalidStateError(
        `Translation to ${language.name} already exists.`,
      );

    if (state.translationLanguages.length > maximumNumberOfTranslations) {
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

  static addFormat(state: UnderEditing, format: Format): FormatAdded {
    const { formatType } = format;

    if (state.formats.some((f) => f.formatType == formatType)) {
      throw InvalidStateError(`Format ${format.formatType} already exists.`);
    }

    return {
      type: 'FormatAdded',
      data: {
        format,
      },
    };
  }

  static removeFormat(state: UnderEditing, format: Format): FormatRemoved {
    const { formatType } = format;

    if (!state.formats.some((f) => f.formatType == formatType)) {
      throw InvalidStateError(`Format ${format.formatType} does not exist.`);
    }

    return {
      type: 'FormatRemoved',
      data: {
        format,
      },
    };
  }

  static addReviewer(state: UnderEditing, reviewer: Reviewer): ReviewerAdded {
    const { id: reviewerId } = reviewer;

    if (state.reviewers.includes(reviewerId)) {
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

  static approve(
    state: UnderEditing,
    committeeApproval: CommitteeApproval,
    minimumReviewersRequiredForApproval: PositiveNumber,
  ): Approved {
    if (state.reviewers.length < minimumReviewersRequiredForApproval) {
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

  static setISBN(state: UnderEditing, isbn: ISBN): ISBNSet {
    if (state.isISBNSet) {
      throw InvalidStateError('Cannot change already set ISBN.');
    }

    return {
      type: 'ISBNSet',
      data: {
        isbn,
      },
    };
  }

  static moveToPrinting(
    state: UnderEditing,
    publishingHouse: IPublishingHouse,
  ): MovedToPrinting {
    if (state.isApproved === null) {
      throw InvalidStateError(
        'Cannot move to printing state until the book has been approved.',
      );
    }

    if (state.genre === null) {
      throw InvalidStateError(
        'Book can be moved to the printing only when genre is specified',
      );
    }

    // Check for genre limit
    if (!publishingHouse.isGenreLimitReached(state.genre)) {
      throw InvalidStateError(
        'Cannot move to printing until the genre limit is reached.',
      );
    }

    const totalCopies = state.formats.reduce(
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
    state: UnderEditing,
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
          state.genre,
          state.isISBNSet,
          state.isApproved,
          state.reviewers,
          [...state.translationLanguages, languageId],
          state.formats,
        );
      }
      case 'TranslationRemoved': {
        const {
          translation: {
            language: { id: languageId },
          },
        } = data;

        return new UnderEditing(
          state.genre,
          state.isISBNSet,
          state.isApproved,
          state.reviewers,
          state.translationLanguages.filter((t) => t != languageId),
          state.formats,
        );
      }
      case 'FormatAdded': {
        const {
          format: { formatType, totalCopies },
        } = data;

        return new UnderEditing(
          state.genre,
          state.isISBNSet,
          state.isApproved,
          state.reviewers,
          state.translationLanguages,
          [...state.formats, { formatType, totalCopies }],
        );
      }
      case 'FormatRemoved': {
        const {
          format: { formatType },
        } = data;

        return new UnderEditing(
          state.genre,
          state.isISBNSet,
          state.isApproved,
          state.reviewers,
          state.translationLanguages,
          state.formats.filter((f) => f.formatType != formatType),
        );
      }
      case 'ReviewerAdded': {
        const {
          reviewer: { id: reviewerId },
        } = data;

        return new UnderEditing(
          state.genre,
          state.isISBNSet,
          state.isApproved,
          [...state.reviewers, reviewerId],
          state.translationLanguages,
          state.formats,
        );
      }
      case 'Approved': {
        return new UnderEditing(
          state.genre,
          state.isISBNSet,
          true,
          state.reviewers,
          state.translationLanguages,
          state.formats,
        );
      }
      case 'ISBNSet': {
        return new UnderEditing(
          state.genre,
          true,
          state.isApproved,
          state.reviewers,
          state.translationLanguages,
          state.formats,
        );
      }
    }
  }

  public static readonly initial = new UnderEditing(
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
