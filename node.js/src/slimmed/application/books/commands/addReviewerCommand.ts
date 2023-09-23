//import { BookId } from '#original';
import { BookId, Reviewer } from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type AddReviewerCommand = Command<
  'AddReviewerCommand',
  {
    bookId: BookId;
    reviewer: Reviewer;
  }
>;
