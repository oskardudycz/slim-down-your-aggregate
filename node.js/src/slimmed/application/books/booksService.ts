import { IBooksRepository } from '../../domain/books/repositories';
import { IAuthorProvider } from '../../domain/books/authors';
import { IPublisherProvider } from '../../domain/books/publishers/publisherProvider';
import {
  Book,
  Draft,
  InPrint,
  Initial,
  PublishedBook,
  UnderEditing,
} from '../../domain/books/book';
import { InvalidOperationError } from '#core/errors';
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
import { BookId } from 'src/original/domain/books/entities';

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
  public createDraft = async (command: CreateDraftCommand): Promise<void> => {
    const { bookId, title, author, publisherId, edition, genre } = command.data;

    const authorEntity = await this.authorProvider.getOrCreate(author);
    const publisherEntity = await this.publisherProvider.getById(publisherId);

    return this.handle(bookId, (book) => {
      if (!(book instanceof Initial))
        throw InvalidOperationError('Invalid State');

      book.createDraft(title, authorEntity, publisherEntity, edition, genre);
    });
  };

  public addChapter = async (command: AddChapterCommand): Promise<void> => {
    return this.handle(command.data.bookId, (book) => {
      if (!(book instanceof Draft))
        throw InvalidOperationError('Invalid State');

      const { chapterTitle, chapterContent } = command.data;

      book.addChapter(chapterTitle, chapterContent);
    });
  };

  public moveToEditing = async (command: MoveToEditingCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof Draft))
        throw InvalidOperationError('Invalid State');

      book.moveToEditing();
    });

  public addTranslation = async (
    command: AddTranslationCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { translation } = command.data;

      book.addTranslation(translation);
    });

  public addFormat = async (command: AddFormatCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { format } = command.data;

      book.addFormat(format);
    });

  public removeFormat = async (command: RemoveFormatCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { format } = command.data;

      book.removeFormat(format);
    });

  public addReviewer = async (command: AddReviewerCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { reviewer } = command.data;

      book.addReviewer(reviewer);
    });

  public approve = async (command: ApproveCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { committeeApproval } = command.data;

      book.approve(committeeApproval);
    });

  public setISBN = async (command: SetISBNCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { isbn } = command.data;

      book.setISBN(isbn);
    });

  public moveToPrinting = async (
    command: MoveToPrintingCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      book.moveToPrinting({ isGenreLimitReached: () => true });
    });

  public moveToPublished = async (
    command: MoveToPublishedCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof InPrint))
        throw InvalidOperationError('Invalid State');

      book.moveToPublished();
    });

  public moveToOutOfPrint = async (
    command: MoveToOutOfPrintCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof PublishedBook))
        throw InvalidOperationError('Invalid State');

      book.moveToOutOfPrint();
    });

  private handle = async (
    id: BookId,
    handle: (book: Book) => void,
  ): Promise<void> => {
    const book = (await this.repository.findById(id)) ?? this.getDefault(id);

    handle(book);

    return this.repository.store(book);
  };

  private getDefault = (bookId: BookId) => new Initial(bookId);

  constructor(
    private repository: IBooksRepository,
    private authorProvider: IAuthorProvider,
    private publisherProvider: IPublisherProvider,
  ) {}
}
