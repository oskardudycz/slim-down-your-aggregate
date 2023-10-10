import { DomainEvent } from '../../../infrastructure/events';
import { BookId } from '../entities';

export class OutOfPrint {
  public get id() {
    return this._id;
  }
  constructor(private readonly _id: BookId) {}

  public static evolve(_: OutOfPrint, event: OutOfPrintEvent): OutOfPrint {
    const { type, data } = event;

    switch (type) {
      case 'MovedToOutOfPrint': {
        const { bookId } = data;
        return new OutOfPrint(bookId);
      }
    }
  }
}

export type MovedToOutOfPrint = DomainEvent<
  'MovedToOutOfPrint',
  {
    bookId: BookId;
  }
>;

export type OutOfPrintEvent = MovedToOutOfPrint;
