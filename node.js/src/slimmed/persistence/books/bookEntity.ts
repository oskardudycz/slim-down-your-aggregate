import { AuthorEntity } from '../authors';
import { PublisherEntity } from '../publishers/publisherEntity';
import { ReviewerEntity } from '../reviewers';
import { ChapterEntity, FormatEntity } from './entities';
import { TranslationEntity } from './entities/translationEntity';
import { CommitteeApprovalVO } from './valueObjects';

export type BookEntity = {
  id: string;
  currentState: State;
  title: string;
  author: AuthorEntity;
  publisher: PublisherEntity;
  edition: number;
  genre?: string | null;
  isbn?: string | null;
  publicationDate?: Date | null;
  totalPages?: number | null;
  numberOfIllustrations?: number | null;
  bindingType?: string | null;
  summary?: string | null;
  committeeApproval?: CommitteeApprovalVO | null;
  reviewers: ReviewerEntity[];
  chapters: ChapterEntity[];
  translations: TranslationEntity[];
  formats: FormatEntity[];
  version: number;
};

export enum State {
  Writing = 'Writing',
  Editing = 'Editing',
  Printing = 'Printing',
  Published = 'Published',
  OutOfPrint = 'OutOfPrint',
}
