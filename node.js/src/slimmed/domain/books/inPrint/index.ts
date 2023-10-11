import { PositiveNumber } from '#core/typing';
import { DomainEvent } from '../../../infrastructure/events';
import { Published } from '../published';

export class InPrint {
  constructor(private totalCopies: PositiveNumber) {}

  moveToPublished(): Published {
    return {
      type: 'Published',
      data: { totalCopies: this.totalCopies },
    };
  }

  public static evolve(_: InPrint, event: InPrintEvent): InPrint {
    const { type, data } = event;

    switch (type) {
      case 'MovedToPrinting': {
        return new InPrint(data.totalCopies);
      }
    }
  }

  public static readonly default = new InPrint(0 as PositiveNumber);
}

export type MovedToPrinting = DomainEvent<
  'MovedToPrinting',
  { totalCopies: PositiveNumber }
>;

export type InPrintEvent = MovedToPrinting;
