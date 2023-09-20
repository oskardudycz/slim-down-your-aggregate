import { ReviewerEntity } from '../../reviewers';

export type BookReviewerEntity = {
  bookId: string;
  reviewer: ReviewerEntity;
};
