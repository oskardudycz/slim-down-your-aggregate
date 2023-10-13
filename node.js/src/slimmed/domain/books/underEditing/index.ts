import { PositiveNumber } from '#core/typing';
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

export * from './decider';

export type UnderEditing = {
  status: 'UnderEditing';
  genre: Genre | null;
  isISBNSet: boolean;
  isApproved: boolean;
  reviewers: ReviewerId[];
  translationLanguages: LanguageId[];
  formats: {
    formatType: FormatType;
    totalCopies: PositiveNumber;
  }[];
};

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

export const evolve = (
  state: UnderEditing,
  event: UnderEditingEvent,
): UnderEditing => {
  const { type, data } = event;

  switch (type) {
    case 'MovedToEditing': {
      const { genre } = data;

      return {
        status: 'UnderEditing',
        genre,
        isISBNSet: false,
        isApproved: false,
        formats: [],
        translationLanguages: [],
        reviewers: [],
      };
    }
    case 'TranslationAdded': {
      const {
        translation: {
          language: { id: languageId },
        },
      } = data;

      return {
        ...state,
        translationLanguages: [...state.translationLanguages, languageId],
      };
    }
    case 'TranslationRemoved': {
      const {
        translation: {
          language: { id: languageId },
        },
      } = data;

      return {
        ...state,
        translationLanguages: state.translationLanguages.filter(
          (t) => t != languageId,
        ),
      };
    }
    case 'FormatAdded': {
      const {
        format: { formatType, totalCopies },
      } = data;

      return {
        ...state,
        formats: [...state.formats, { formatType, totalCopies }],
      };
    }
    case 'FormatRemoved': {
      const {
        format: { formatType },
      } = data;

      return {
        ...state,
        formats: state.formats.filter((f) => f.formatType != formatType),
      };
    }
    case 'ReviewerAdded': {
      const {
        reviewer: { id: reviewerId },
      } = data;

      return {
        ...state,
        reviewers: [...state.reviewers, reviewerId],
      };
    }
    case 'Approved': {
      return {
        ...state,
        isApproved: true,
      };
    }
    case 'ISBNSet': {
      return {
        ...state,
        isISBNSet: true,
      };
    }
  }
};

export const initial: UnderEditing = {
  status: 'UnderEditing',
  genre: null,
  isApproved: false,
  isISBNSet: false,
  reviewers: [],
  translationLanguages: [],
  formats: [],
};

export const isUnderEditing = (obj: object): obj is UnderEditing =>
  'status' in obj &&
  typeof obj.status === 'string' &&
  obj.status === 'UnderEditing';
