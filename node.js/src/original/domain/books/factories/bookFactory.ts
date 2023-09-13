import { List } from 'immutable';
import { State, Book } from '../book';
import {
  Author,
  BookId,
  Chapter,
  CommitteeApproval,
  Format,
  Genre,
  ISBN,
  Publisher,
  Reviewer,
  Title,
  Translation,
} from '../entities';
import { NonEmptyString, PositiveNumber } from '#core/typing';
import { IPublishingHouse } from '../services/publishingHouse';

export interface IBookFactory {
  create(
    bookId: BookId,
    state: State,
    title: Title,
    author: Author,
    publishingHouse: IPublishingHouse,
    publisher: Publisher,
    edition: PositiveNumber,
    genre: Genre | null,
    isbn: ISBN | null,
    publicationDate: Date | null,
    totalPages: PositiveNumber | null,
    numberOfIllustrations: PositiveNumber | null,
    bindingType: NonEmptyString | null,
    summary: NonEmptyString | null,
    committeeApproval: CommitteeApproval | null,
    reviewers: Reviewer[] | null,
    chapters: Chapter[] | null,
    translations: Translation[] | null,
    formats: Format[] | null,
  ): Book;
}
