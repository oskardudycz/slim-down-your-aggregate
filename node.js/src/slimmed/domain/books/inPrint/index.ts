import { DomainEvent } from '../../../infrastructure/events';
import { BookId } from '../entities';
import { Published } from '../published';

export class InPrint {
  public get id() {
    return this._id;
  }
  constructor(private _id: BookId) {}

  moveToPublished(): Published {
    return {
      type: 'Published',
      data: {
        bookId: this.id,
      },
    };
  }

  public static evolve(_: InPrint, event: InPrintEvent): InPrint {
    const { type, data } = event;

    switch (type) {
      case 'MovedToPrinting': {
        const { bookId } = data;

        return new InPrint(bookId);
      }
    }
  }
}

export type MovedToPrinting = DomainEvent<
  'MovedToPrinting',
  {
    bookId: BookId;
  }
>;

export type InPrintEvent = MovedToPrinting;
