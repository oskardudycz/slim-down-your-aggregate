import {
  DEFAULT_POSITIVE_NUMBER,
  PositiveNumber,
  positiveNumber,
} from '#core/typing';
import { DeepReadonly } from 'ts-essentials';
import { DomainEvent } from '../../../infrastructure/events';

export * from './decider';

export type PublishedBook = DeepReadonly<{
  status: 'Published';
  totalCopies: PositiveNumber;
  totalSoldCopies: PositiveNumber;
}>;

export const initial: PublishedBook = {
  status: 'Published',
  totalCopies: DEFAULT_POSITIVE_NUMBER,
  totalSoldCopies: DEFAULT_POSITIVE_NUMBER,
};

export type Published = DomainEvent<
  'Published',
  { totalCopies: PositiveNumber }
>;

export type PublishedEvent = Published;

export const evolve = (
  book: PublishedBook,
  event: PublishedEvent,
): PublishedBook => {
  const { type, data } = event;

  switch (type) {
    case 'Published': {
      // TODO: Add methods to set sold copies
      return {
        status: 'Published',
        totalCopies: data.totalCopies,
        totalSoldCopies: positiveNumber(1),
      };
    }
  }
};

export const isPublished = (obj: object): obj is PublishedBook =>
  'status' in obj &&
  typeof obj.status === 'string' &&
  obj.status === 'Published';
