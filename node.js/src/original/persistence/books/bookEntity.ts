import { AuthorEntity } from '../authors';
import { PublisherEntity } from '../publishers/publisherEntity';
import { ReviewerEntity } from '../reviewers';
import { ChapterEntity, FormatEntity } from './entities';
import { CommitteeApprovalVO, TranslationVO } from './valueObjects';

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
  numberOfIllustrations: number | null;
  bindingType?: string | null;
  summary?: string | null;
  cmmitteeApproval?: CommitteeApprovalVO | null;
  reviewers: ReviewerEntity[];
  chapters: ChapterEntity[];
  translations: TranslationVO[];
  formats: FormatEntity[];
};

export enum State {
  Writing,
  Editing,
  Printing,
  Published,
  OutOfPrint,
}
