import { DomainEvent, EmptyData } from '../../../infrastructure/events';

export class OutOfPrint {
  public static evolve(_: OutOfPrint, event: OutOfPrintEvent): OutOfPrint {
    const { type } = event;

    switch (type) {
      case 'MovedToOutOfPrint': {
        return new OutOfPrint();
      }
    }
  }

  public static readonly initial = new OutOfPrint();
}

export type MovedToOutOfPrint = DomainEvent<'MovedToOutOfPrint', EmptyData>;

export type OutOfPrintEvent = MovedToOutOfPrint;
