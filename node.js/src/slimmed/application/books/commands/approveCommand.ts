//import { BookId } from '#original';
import { BookId, CommitteeApproval } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type ApproveCommand = Command<
  'ApproveCommand',
  {
    bookId: BookId;
    committeeApproval: CommitteeApproval;
  }
>;
