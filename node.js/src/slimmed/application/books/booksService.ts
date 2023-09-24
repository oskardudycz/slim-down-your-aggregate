import { IBooksRepository } from '../../persistence/books/repositories';
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
import { PositiveNumber } from '#core/typing';
import { Ratio } from '#core/typing/ratio';
import { BookId } from '../../domain/books/entities';
import { BookEvent } from '../../domain/books/bookEvent';
import { IBookFactory } from '../../domain/books/factories';
import { bookMapper } from '../../persistence/mappers/bookMapper';

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

      return book.createDraft(
        title,
        authorEntity,
        publisherEntity,
        edition,
        genre,
      );
    });
  };

  public addChapter = async (command: AddChapterCommand): Promise<void> => {
    return this.handle(command.data.bookId, (book) => {
      if (!(book instanceof Draft))
        throw InvalidOperationError('Invalid State');

      const { chapterTitle, chapterContent } = command.data;

      return book.addChapter(chapterTitle, chapterContent);
    });
  };

  public moveToEditing = async (command: MoveToEditingCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof Draft))
        throw InvalidOperationError('Invalid State');

      return book.moveToEditing();
    });

  public addTranslation = async (
    command: AddTranslationCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { translation } = command.data;

      return book.addTranslation(translation, this.maximumNumberOfTranslations);
    });

  public addFormat = async (command: AddFormatCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { format } = command.data;

      return book.addFormat(format);
    });

  public removeFormat = async (command: RemoveFormatCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { format } = command.data;

      return book.removeFormat(format);
    });

  public addReviewer = async (command: AddReviewerCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { reviewer } = command.data;

      return book.addReviewer(reviewer);
    });

  public approve = async (command: ApproveCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { committeeApproval } = command.data;

      return book.approve(
        committeeApproval,
        this.minimumReviewersRequiredForApproval,
      );
    });

  public setISBN = async (command: SetISBNCommand): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      const { isbn } = command.data;

      return book.setISBN(isbn);
    });

  public moveToPrinting = async (
    command: MoveToPrintingCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof UnderEditing))
        throw InvalidOperationError('Invalid State');

      return book.moveToPrinting({ isGenreLimitReached: () => true });
    });

  public moveToPublished = async (
    command: MoveToPublishedCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof InPrint))
        throw InvalidOperationError('Invalid State');

      return book.moveToPublished();
    });

  public moveToOutOfPrint = async (
    command: MoveToOutOfPrintCommand,
  ): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!(book instanceof PublishedBook))
        throw InvalidOperationError('Invalid State');

      return book.moveToOutOfPrint(
        this.maxAllowedUnsoldCopiesRatioToGoOutOfPrint,
      );
    });

  private handle = (
    id: BookId,
    handle: (book: Book) => BookEvent | BookEvent[],
  ): Promise<void> => {
    return this.repository.getAndUpdate(id, (entity) => {
      const aggregate =
        entity !== null
          ? bookMapper.mapFromEntity(entity, this.bookFactory)
          : this.getDefault(id);

      const result = handle(aggregate);
      return Array.isArray(result) ? result : [result];
    });
  };

  private getDefault = (bookId: BookId): Book => new Initial(bookId);

  constructor(
    private readonly repository: IBooksRepository,
    private readonly bookFactory: IBookFactory,
    private readonly authorProvider: IAuthorProvider,
    private readonly publisherProvider: IPublisherProvider,
    private readonly minimumReviewersRequiredForApproval: PositiveNumber,
    private readonly maximumNumberOfTranslations: PositiveNumber,
    private readonly maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio,
  ) {}
}
