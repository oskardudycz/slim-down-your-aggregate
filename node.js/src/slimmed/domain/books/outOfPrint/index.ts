import { DeepReadonly } from 'ts-essentials';
import { DomainEvent, EmptyData } from '../../../infrastructure/events';

export type OutOfPrint = DeepReadonly<{ status: 'OutOfPrint' }>;

export const initial: OutOfPrint = { status: 'OutOfPrint' };

export type MovedToOutOfPrint = DomainEvent<'MovedToOutOfPrint', EmptyData>;

export type OutOfPrintEvent = MovedToOutOfPrint;

export const evolve = (_: OutOfPrint, event: OutOfPrintEvent): OutOfPrint => {
  const { type } = event;

  switch (type) {
    case 'MovedToOutOfPrint': {
      return { status: 'OutOfPrint' };
    }
  }
};

export const isOutOfPrint = (obj: object): obj is OutOfPrint =>
  'status' in obj &&
  typeof obj.status === 'string' &&
  obj.status === 'OutOfPrint';
