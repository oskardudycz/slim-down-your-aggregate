import { NonEmptyString, PositiveNumber } from '#core/typing';
import { Aggregate } from '../../infrastructure/aggregates';
import {
  Author,
  Chapter,
  CommitteeApproval,
  Format,
  Genre,
  ISBN,
  Publisher,
  Reviewer,
  Title,
  Translation,
} from './entities';
import { BookId } from './entities/bookId';
import { IPublishingHouse } from './services/publishingHouse';

export class Book extends Aggregate<BookId> {
  #currentState: State;
  #title: Title;
  #author: Author;
  #publisher: Publisher;
  #genre: Genre | undefined;
  #isbn: ISBN | undefined;
  #publicationDate: Date | undefined;
  #totalPages: PositiveNumber | undefined;
  #numberOfIllustrations?: PositiveNumber | undefined;
  #bindingType: NonEmptyString | undefined;
  #summary: NonEmptyString | undefined;
  #committeeApproval: CommitteeApproval | undefined;
  #publishingHouse: IPublishingHouse | undefined;
  #reviewers: Reviewer[];
  #chapters: Chapter[];
  #translations: Translation[];
  #formats: Format[];

  public get currentState() {
    return this.#currentState;
  }
  public get title() {
    return this.#title;
  }
  public get author() {
    return this.#author;
  }
  public get publisher() {
    return this.#publisher;
  }
  public get genre() {
    return this.#genre;
  }
  public get isbn() {
    return this.#isbn;
  }
  public get publicationDate() {
    return this.#publicationDate;
  }
  public get totalPages() {
    return this.#totalPages;
  }
  public get numberOfIllustrations() {
    return this.#numberOfIllustrations;
  }
  public get bindingType() {
    return this.#bindingType;
  }
  public get summary() {
    return this.#summary;
  }
  public get committeeApproval() {
    return this.#committeeApproval;
  }
  public get reviewers() {
    return [...this.#reviewers];
  }
  public get chapters() {
    return [...this.#chapters];
  }
  public get translations() {
    return [...this.#translations];
  }
  public get formats() {
    return [...this.#formats];
  }

  private constructor(
    id: BookId,
    currentState: State,
    title: Title,
    author: Author,
    publisher: Publisher,
    genre: Genre | undefined,
    isbn: ISBN | undefined,
    publicationDate: Date | undefined,
    totalPages: PositiveNumber | undefined,
    numberOfIllustrations: PositiveNumber | undefined,
    bindingType: NonEmptyString | undefined,
    summary: NonEmptyString | undefined,
    committeeApproval: CommitteeApproval | undefined,
    publishingHouse: IPublishingHouse | undefined,
    reviewers: Reviewer[],
    chapters: Chapter[],
    translations: Translation[],
    formats: Format[],
  ) {
    super(id);
    this.#currentState = currentState;
    this.#title = title;
    this.#author = author;
    (this.#publisher = publisher), (this.#genre = genre);
    this.#isbn = isbn;
    this.#publicationDate = publicationDate;
    this.#totalPages = totalPages;
    this.#numberOfIllustrations = numberOfIllustrations;
    this.#bindingType = bindingType;
    this.#summary = summary;
    this.#committeeApproval = committeeApproval;
    this.#publishingHouse = publishingHouse;
    this.#reviewers = reviewers;
    this.#chapters = chapters;
    this.#translations = translations;
    this.#formats = formats;
  }
}

export enum State {
  Writing,
  Editing,
  Printing,
  Published,
  OutOfPrint,
}
