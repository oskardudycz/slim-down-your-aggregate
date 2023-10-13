import { Command } from '../../../infrastructure/commands';
import { InPrint } from '.';
import { BookId } from '../entities';
import { Published } from '../published';

export type MoveToPublished = Command<
  'MoveToPublishedCommand',
  {
    bookId: BookId;
  }
>;

export type InPrintCommand = MoveToPublished;

export const moveToPublished = (
  command: MoveToPublished,
  state: InPrint,
): Published => {
  return {
    type: 'Published',
    data: { totalCopies: state.totalCopies },
  };
};
