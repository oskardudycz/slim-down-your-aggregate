import { InvalidStateError } from '#core/errors';
import { Ratio, ratio } from '#core/typing/ratio';
import { Command } from '../../../infrastructure/commands';
import { PublishedBook } from '.';
import { BookId } from '../entities';
import { MovedToOutOfPrint } from '../outOfPrint';

export type MoveToOutOfPrint = Command<
  'MoveToOutOfPrintCommand',
  {
    bookId: BookId;
  }
>;

export type PublishedCommand = MoveToOutOfPrint;

const unsoldCopiesRatio = (state: PublishedBook): Ratio => {
  return ratio((state.totalCopies - state.totalSoldCopies) / state.totalCopies);
};

export const moveToOutOfPrint = (
  state: PublishedBook,
  maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio,
): MovedToOutOfPrint => {
  if (unsoldCopiesRatio(state) > maxAllowedUnsoldCopiesRatioToGoOutOfPrint) {
    throw InvalidStateError(
      'Cannot move to Out of Print state if more than 10% of total copies are unsold.',
    );
  }

  return {
    type: 'MovedToOutOfPrint',
    data: {},
  };
};
