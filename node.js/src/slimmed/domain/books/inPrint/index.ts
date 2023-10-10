import { DomainEvent, EmptyData } from '../../../infrastructure/events';
import { Published } from '../published';

export class InPrint {
  moveToPublished(): Published {
    return {
      type: 'Published',
      data: {},
    };
  }

  public static evolve(_: InPrint, event: InPrintEvent): InPrint {
    const { type } = event;

    switch (type) {
      case 'MovedToPrinting': {
        return new InPrint();
      }
    }
  }
}

export type MovedToPrinting = DomainEvent<'MovedToPrinting', EmptyData>;

export type InPrintEvent = MovedToPrinting;
