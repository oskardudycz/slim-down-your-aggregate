export type BookDetails = {
  id: string;
  currentState: string;
  title: string;
  author: AuthorDetails;
  publisherName: string;
  edition: number;
  genre?: string | null;
  isbn?: string | null;
  publicationDate?: string | null;
  totalPages?: number | null;
  numberOfIllustrations?: number | null;
  bindingType?: string | null;
  summary?: string | null;
  committeeApproval: CommitteeApprovalDetails | null;
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
