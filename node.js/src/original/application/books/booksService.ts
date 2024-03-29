import { IBooksRepository } from '../../domain/books/repositories';
import { IAuthorProvider } from '../../domain/books/authors';
import { IPublisherProvider } from '../../domain/books/publishers/publisherProvider';
import { Book } from '../../domain/books/book';
import { IPublishingHouse } from '../../domain/books/services';
import { NotFoundError } from '#core/errors';
import {
  AddChapterCommand,
  AddFormatCommand,
  AddTranslationCommand,
  ApproveCommand,
  CreateDraftCommand,
  MoveToEditingCommand,
  MoveToOutOfPrintCommand,
  MoveToPrintingCommand,
  MoveToPublishedCommand,
  RemoveFormatCommand,
  AddReviewerCommand,
  SetISBNCommand,
} from './commands';

export interface IBooksService {
  createDraft(command: CreateDraftCommand): Promise<void>;
  addChapter(command: AddChapterCommand): Promise<void>;
  moveToEditing(command: MoveToEditingCommand): Promise<void>;
  addTranslation(command: AddTranslationCommand): Promise<void>;
  addFormat(command: AddFormatCommand): Promise<void>;
  removeFormat(command: RemoveFormatCommand): Promise<void>;
  addReviewer(command: AddReviewerCommand): Promise<void>;
  approve(command: ApproveCommand): Promise<void>;
  setISBN(command: SetISBNCommand): Promise<void>;
  moveToPrinting(command: MoveToPrintingCommand): Promise<void>;
  moveToPublished(command: MoveToPublishedCommand): Promise<void>;
  moveToOutOfPrint(command: MoveToOutOfPrintCommand): Promise<void>;
}

export class BooksService implements IBooksService {
  async createDraft(command: CreateDraftCommand): Promise<void> {
    const { bookId, title, author, publisherId, edition, genre } = command.data;

    const book = Book.createDraft(
      bookId,
      title,
      await this.authorProvider.getOrCreate(author),
      {} as IPublishingHouse,
      await this.publisherProvider.getById(publisherId),
      edition,
      genre,
    );

    return this.repository.add(book);
  }

  async addChapter(command: AddChapterCommand): Promise<void> {
    const { bookId, chapterTitle, chapterContent } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.addChapter(chapterTitle, chapterContent);

    return this.repository.update(book);
  }

  async moveToEditing(command: MoveToEditingCommand): Promise<void> {
    const book = await this.repository.findById(command.data.bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.moveToEditing();

    return this.repository.update(book);
  }

  async addTranslation(command: AddTranslationCommand): Promise<void> {
    const { bookId, translation } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.addTranslation(translation);

    return this.repository.update(book);
  }

  async addFormat(command: AddFormatCommand): Promise<void> {
    const { bookId, format } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.addFormat(format);

    return this.repository.update(book);
  }

  async removeFormat(command: RemoveFormatCommand): Promise<void> {
    const { bookId, format } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.removeFormat(format);

    return this.repository.update(book);
  }

  async addReviewer(command: AddReviewerCommand): Promise<void> {
    const { bookId, reviewer } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.addReviewer(reviewer);

    return this.repository.update(book);
  }

  async approve(command: ApproveCommand): Promise<void> {
    const { bookId, committeeApproval } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.approve(committeeApproval);

    return this.repository.update(book);
  }

  async setISBN(command: SetISBNCommand): Promise<void> {
    const { bookId, isbn } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.setISBN(isbn);

    return this.repository.update(book);
  }

  async moveToPrinting(command: MoveToPrintingCommand): Promise<void> {
    const { bookId } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.moveToPrinting();

    return this.repository.update(book);
  }

  async moveToPublished(command: MoveToPublishedCommand): Promise<void> {
    const { bookId } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.moveToPublished();

    return this.repository.update(book);
  }

  async moveToOutOfPrint(command: MoveToOutOfPrintCommand): Promise<void> {
    const { bookId } = command.data;

    const book = await this.repository.findById(bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    book.moveToOutOfPrint();

    return this.repository.update(book);
  }

  constructor(
    private repository: IBooksRepository,
    private authorProvider: IAuthorProvider,
    private publisherProvider: IPublisherProvider,
  ) {}
}
