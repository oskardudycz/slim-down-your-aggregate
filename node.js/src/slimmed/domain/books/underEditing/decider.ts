import { InvalidStateError } from '#core/errors';
import { PositiveNumber, positiveNumber } from '#core/typing';
import {
  UnderEditing,
  TranslationAdded,
  FormatAdded,
  FormatRemoved,
  ReviewerAdded,
  Approved,
  ISBNSet,
} from '.';
import {
  Translation,
  Reviewer,
  CommitteeApproval,
  ISBN,
  Format,
} from '../entities';
import { MovedToPrinting } from '../inPrint';
import { IPublishingHouse } from '../services';

export const addTranslation = (
  state: UnderEditing,
  translation: Translation,
  maximumNumberOfTranslations: PositiveNumber,
): TranslationAdded => {
  const { language } = translation;

  if (state.translationLanguages.includes(language.id))
    throw InvalidStateError(`Translation to ${language.name} already exists.`);

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
};

export const addFormat = (state: UnderEditing, format: Format): FormatAdded => {
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
};

export const removeFormat = (
  state: UnderEditing,
  format: Format,
): FormatRemoved => {
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
};

export const addReviewer = (
  state: UnderEditing,
  reviewer: Reviewer,
): ReviewerAdded => {
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
};

export const approve = (
  state: UnderEditing,
  committeeApproval: CommitteeApproval,
  minimumReviewersRequiredForApproval: PositiveNumber,
): Approved => {
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
};

export const setISBN = (state: UnderEditing, isbn: ISBN): ISBNSet => {
  if (state.isISBNSet) {
    throw InvalidStateError('Cannot change already set ISBN.');
  }

  return {
    type: 'ISBNSet',
    data: {
      isbn,
    },
  };
};

export const moveToPrinting = (
  state: UnderEditing,
  publishingHouse: IPublishingHouse,
): MovedToPrinting => {
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
};
