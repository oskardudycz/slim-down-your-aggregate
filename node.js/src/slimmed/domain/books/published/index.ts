import { InvalidStateError } from '#core/errors';
import { PositiveNumber, positiveNumber } from '#core/typing';
import { Ratio, ratio } from '#core/typing/ratio';
import { DomainEvent } from '../../../infrastructure/events';
import { MovedToOutOfPrint } from '../outOfPrint';

export class PublishedBook {
  constructor(
    private readonly totalCopies: PositiveNumber,
    private readonly totalSoldCopies: PositiveNumber,
  ) {}

  private get unsoldCopiesRatio(): Ratio {
    return ratio((this.totalCopies - this.totalSoldCopies) / this.totalCopies);
  }

  moveToOutOfPrint(
    maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio,
  ): MovedToOutOfPrint {
    if (this.unsoldCopiesRatio > maxAllowedUnsoldCopiesRatioToGoOutOfPrint) {
      throw InvalidStateError(
        'Cannot move to Out of Print state if more than 10% of total copies are unsold.',
      );
    }

    return {
      type: 'MovedToOutOfPrint',
      data: {},
    };
  }

  public static evolve(
    book: PublishedBook,
    event: PublishedEvent,
  ): PublishedBook {
    const { type, data } = event;

    switch (type) {
      case 'Published': {
        // TODO: Add methods to set sold copies
        return new PublishedBook(data.totalCopies, positiveNumber(1));
      }
    }
  }

  public static readonly default = new PublishedBook(
    positiveNumber(1),
    positiveNumber(1),
  );
}

export type Published = DomainEvent<
  'Published',
  { totalCopies: PositiveNumber }
>;

export type PublishedEvent = Published;
