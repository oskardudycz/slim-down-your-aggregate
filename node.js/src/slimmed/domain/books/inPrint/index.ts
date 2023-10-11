import { DEFAULT_POSITIVE_NUMBER, PositiveNumber } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';
import { DomainEvent } from '../../../infrastructure/events';

export * from './decider';

export type InPrint = DeepReadonly<{
  status: 'InPrint';
  totalCopies: PositiveNumber;
}>;

export const initial: InPrint = {
  status: 'InPrint',
  totalCopies: DEFAULT_POSITIVE_NUMBER,
};

export type MovedToPrinting = DomainEvent<
  'MovedToPrinting',
  { totalCopies: PositiveNumber }
>;

export type InPrintEvent = MovedToPrinting;

export const evolve = (_: InPrint, event: InPrintEvent): InPrint => {
  const { type, data } = event;

  switch (type) {
    case 'MovedToPrinting': {
      return {
        status: 'InPrint',
        totalCopies: data.totalCopies,
      };
    }
  }
};

export const isInPrint = (obj: object): obj is InPrint =>
  'status' in obj && typeof obj.status === 'string' && obj.status === 'InPrint';
