export type BookDetails = {
  id: string;
  currentState: string;
  title: string;
  author: AuthorDetails;
  publisherName: string;
  edition: number;
  genre?: string | undefined;
  isbn?: string | undefined;
  publicationDate?: string | undefined;
  totalPages?: number | undefined;
  numberOfIllustrations?: number | undefined;
  bindingType?: string | undefined;
  summary?: string | undefined;
  committeeApproval: CommitteeApprovalDetails | undefined;
  reviewers: string[];
  chapters: ChapterDetails[];
  translations: TranslationDetails[];
  formats: FormatDetails[];
};

export type AuthorDetails = { firstName: string; lastName: string };
export type CommitteeApprovalDetails = {
  isApproved: boolean;
  feedback: string;
};
export type ChapterDetails = { title: string; content: string };
export type TranslationDetails = { language: string; translator: string };
export type FormatDetails = {
  formatType: string;
  totalCopies: number;
  soldCopies: number;
};
