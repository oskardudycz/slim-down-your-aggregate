import { InvalidStateError } from '#core/errors';
import { PositiveNumber, positiveNumber } from '#core/typing';
import { Command } from '../../../infrastructure/commands';
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
  BookId,
} from '../entities';
import { MovedToPrinting } from '../inPrint';

export type AddTranslation = Command<
  'AddTranslation',
  {
    bookId: BookId;
    translation: Translation;
    maximumNumberOfTranslations: PositiveNumber;
  }
>;

export type AddFormat = Command<
  'AddFormat',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type RemoveFormat = Command<
  'RemoveFormat',
  {
    bookId: BookId;
    format: Format;
  }
>;

export type AddReviewer = Command<
  'AddReviewer',
  {
    bookId: BookId;
    reviewer: Reviewer;
  }
>;

export type Approve = Command<
  'Approve',
  {
    bookId: BookId;
    committeeApproval: CommitteeApproval;
    minimumReviewersRequiredForApproval: PositiveNumber;
  }
>;

export type SetISBN = Command<
  'SetISBN',
  {
    bookId: BookId;
    isbn: ISBN;
  }
>;

export type MoveToPrinting = Command<
  'MoveToPrinting',
  {
    bookId: BookId;
  }
>;

export type UnderEdititngCommand =
  | AddTranslation
  | AddFormat
  | RemoveFormat
  | AddReviewer
  | Approve
  | SetISBN
  | MoveToPrinting;

export const addTranslation = (
  command: AddTranslation,
  state: UnderEditing,
): TranslationAdded => {
  const {
    translation,
    maximumNumberOfTranslations,
    translation: { language },
  } = command.data;

  if (state.translationLanguages.length > maximumNumberOfTranslations) {
    throw InvalidStateError(
      `Cannot add more translations. Maximum ${maximumNumberOfTranslations} translations are allowed.`,
    );
  }

  if (state.translationLanguages.includes(language.id))
    throw InvalidStateError(`Translation to ${language.name} already exists.`);

  return {
    type: 'TranslationAdded',
    data: {
      translation,
    },
  };
};

export const addFormat = (
  command: AddFormat,
  state: UnderEditing,
): FormatAdded => {
  const {
    format,
    format: { formatType },
  } = command.data;

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
  command: RemoveFormat,
  state: UnderEditing,
): FormatRemoved => {
  const {
    format,
    format: { formatType },
  } = command.data;

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
  command: AddReviewer,
  state: UnderEditing,
): ReviewerAdded => {
  const {
    reviewer,
    reviewer: { id: reviewerId, name: reviewerName },
  } = command.data;

  if (state.reviewers.includes(reviewerId)) {
    throw InvalidStateError(`${reviewerName} is already a reviewer.`);
  }

  return {
    type: 'ReviewerAdded',
    data: {
      reviewer,
    },
  };
};

export const approve = (command: Approve, state: UnderEditing): Approved => {
  const { committeeApproval, minimumReviewersRequiredForApproval } =
    command.data;

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

export const setISBN = (command: SetISBN, state: UnderEditing): ISBNSet => {
  const { isbn } = command.data;

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
  command: MoveToPrinting,
  state: UnderEditing,
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
