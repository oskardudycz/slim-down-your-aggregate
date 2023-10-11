import { InvalidStateError } from '#core/errors';
import {
  DEFAULT_POSITIVE_NUMBER,
  PositiveNumber,
  positiveNumber,
} from '#core/typing';
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

  static moveToOutOfPrint(
    state: PublishedBook,
    maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio,
  ): MovedToOutOfPrint {
    if (state.unsoldCopiesRatio > maxAllowedUnsoldCopiesRatioToGoOutOfPrint) {
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

  public static readonly initial = new PublishedBook(
    DEFAULT_POSITIVE_NUMBER,
    DEFAULT_POSITIVE_NUMBER,
  );
}

export type Published = DomainEvent<
  'Published',
  { totalCopies: PositiveNumber }
>;

export type PublishedEvent = Published;
