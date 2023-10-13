import { IBooksRepository } from '../../persistence/books/repositories';
import { AuthorIdOrData, IAuthorProvider } from '../../domain/books/authors';
import { IPublisherProvider } from '../../domain/books/publishers/publisherProvider';
import { Book } from '../../domain/books/book';
import { InvalidOperationError, InvalidStateError } from '#core/errors';
import { PositiveNumber } from '#core/typing';
import { Ratio } from '#core/typing/ratio';
import {
  BookId,
  CommitteeApproval,
  Genre,
  PublisherId,
  Title,
  Translation,
} from '../../domain/books/entities';
import { BookEvent } from '../../domain/books/book';
import { IBookFactory } from '../../domain/books/factories';
import { bookMapper } from '../../persistence/mappers/bookMapper';
import { initial, isDraft, isInitial } from '../../domain/books/draft';
import {
  MoveToPublished,
  isInPrint,
  moveToPublished,
} from '../../domain/books/inPrint';
import {
  MoveToOutOfPrint as MoveToOutOfPrintWithAdditionalData,
  isPublished,
  moveToOutOfPrint,
} from '../../domain/books/published';
import {
  AddFormat,
  AddReviewer,
  AddTranslation as AddTranslationWithAdditonalData,
  Approve as ApproveWithAdditionalData,
  MoveToPrinting,
  RemoveFormat,
  SetISBN,
  addFormat,
  addReviewer,
  addTranslation,
  approve,
  isUnderEditing,
  moveToPrinting,
  removeFormat,
  setISBN,
} from '../../domain/books/underEditing';
import {
  AddChapter,
  MoveToEditing,
  addChapter,
  createDraft,
  moveToEditing,
} from 'src/slimmed/domain/books/draft';
import { Command } from '../../infrastructure/commands';
import { IPublishingHouse } from 'src/original/domain/books/services';

export interface IBooksService {
  createDraft(command: CreateDraftAndSetupAuthorAndPublisher): Promise<void>;
  addChapter(command: AddChapter): Promise<void>;
  moveToEditing(command: MoveToEditing): Promise<void>;
  addTranslation(command: AddTranslation): Promise<void>;
  addFormat(command: AddFormat): Promise<void>;
  removeFormat(command: RemoveFormat): Promise<void>;
  addReviewer(command: AddReviewer): Promise<void>;
  approve(command: Approve): Promise<void>;
  setISBN(command: SetISBN): Promise<void>;
  moveToPrinting(command: MoveToPrinting): Promise<void>;
  moveToPublished(command: MoveToPublished): Promise<void>;
  moveToOutOfPrint(command: MoveToOutOfPrint): Promise<void>;
}

export type CreateDraftAndSetupAuthorAndPublisher = Command<
  'CreateDraftCommand',
  {
    bookId: BookId;
    title: Title;
    author: AuthorIdOrData;
    publisherId: PublisherId;
    edition: PositiveNumber;
    genre: Genre | null;
  }
>;

export type AddTranslation = Command<
  'AddTranslationCommand',
  {
    bookId: BookId;
    translation: Translation;
  }
>;
export type Approve = Command<
  'ApproveCommand',
  {
    bookId: BookId;
    committeeApproval: CommitteeApproval;
  }
>;

export type MoveToOutOfPrint = Command<
  'MoveToOutOfPrintCommand',
  {
    bookId: BookId;
    maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio;
  }
>;

export type BookApplicationCommand =
  | CreateDraftAndSetupAuthorAndPublisher
  | AddTranslation
  | Approve;

export class BooksService implements IBooksService {
  public createDraft = async (
    command: CreateDraftAndSetupAuthorAndPublisher,
  ): Promise<void> => {
    const { bookId, title, author, publisherId, edition, genre } = command.data;

    const authorEntity = await this.authorProvider.getOrCreate(author);
    const publisherEntity = await this.publisherProvider.getById(publisherId);

    return this.handle(bookId, (book) => {
      if (!isInitial(book)) throw InvalidOperationError('Invalid State');

      return createDraft(
        book,
        title,
        authorEntity,
        publisherEntity,
        edition,
        genre,
      );
    });
  };

  public addChapter = async (command: AddChapter): Promise<void> => {
    return this.handle(command.data.bookId, (book) => {
      if (!isDraft(book)) throw InvalidOperationError('Invalid State');

      const { chapterTitle, chapterContent } = command.data;

      return addChapter(book, chapterTitle, chapterContent);
    });
  };

  public moveToEditing = async (command: MoveToEditing): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!isDraft(book)) throw InvalidOperationError('Invalid State');

      return moveToEditing(book);
    });

  public addTranslation = async (
    applicationCommand: AddTranslation,
  ): Promise<void> => {
    const command: AddTranslationWithAdditonalData = {
      type: 'AddTranslation',
      data: {
        ...applicationCommand.data,
        maximumNumberOfTranslations: this.maximumNumberOfTranslations,
      },
    };

    return this.handle(command.data.bookId, (book) => {
      if (!isUnderEditing(book)) throw InvalidOperationError('Invalid State');

      return addTranslation(command, book);
    });
  };

  public addFormat = async (command: AddFormat): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!isUnderEditing(book)) throw InvalidOperationError('Invalid State');

      return addFormat(command, book);
    });

  public removeFormat = async (command: RemoveFormat): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!isUnderEditing(book)) throw InvalidOperationError('Invalid State');

      return removeFormat(command, book);
    });

  public addReviewer = async (command: AddReviewer): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!isUnderEditing(book)) throw InvalidOperationError('Invalid State');

      return addReviewer(command, book);
    });

  public approve = async (applicationCommand: Approve): Promise<void> => {
    const command: ApproveWithAdditionalData = {
      type: 'Approve',
      data: {
        ...applicationCommand.data,
        minimumReviewersRequiredForApproval:
          this.minimumReviewersRequiredForApproval,
      },
    };

    return this.handle(command.data.bookId, (book) => {
      if (!isUnderEditing(book)) throw InvalidOperationError('Invalid State');

      return approve(command, book);
    });
  };

  public setISBN = async (command: SetISBN): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!isUnderEditing(book)) throw InvalidOperationError('Invalid State');

      return setISBN(command, book);
    });

  public moveToPrinting = async (command: MoveToPrinting): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!isUnderEditing(book)) throw InvalidOperationError('Invalid State');

      if (book.genre && !this.publishingHouse.isGenreLimitReached(book.genre)) {
        throw InvalidStateError(
          'Cannot move to printing until the genre limit is reached.',
        );
      }

      return moveToPrinting(command, book);
    });

  public moveToPublished = async (command: MoveToPublished): Promise<void> =>
    this.handle(command.data.bookId, (book) => {
      if (!isInPrint(book)) throw InvalidOperationError('Invalid State');

      return moveToPublished(command, book);
    });

  public moveToOutOfPrint = async (
    applicationCommand: MoveToOutOfPrint,
  ): Promise<void> => {
    const command: MoveToOutOfPrintWithAdditionalData = {
      type: 'MoveToOutOfPrintCommand',
      data: {
        ...applicationCommand.data,
        maxAllowedUnsoldCopiesRatioToGoOutOfPrint:
          this.maxAllowedUnsoldCopiesRatioToGoOutOfPrint,
      },
    };

    return this.handle(command.data.bookId, (book) => {
      if (!isPublished(book)) throw InvalidOperationError('Invalid State');

      return moveToOutOfPrint(command, book);
    });
  };

  private handle = (
    id: BookId,
    handle: (book: Book) => BookEvent | BookEvent[],
  ): Promise<void> => {
    return this.repository.getAndUpdate(id, (entity) => {
      const aggregate =
        entity !== null
          ? bookMapper.mapFromEntity(entity, this.bookFactory)
          : this.getDefault();

      const result = handle(aggregate);
      return Array.isArray(result) ? result : [result];
    });
  };

  private getDefault = (): Book => initial;

  constructor(
    private readonly repository: IBooksRepository,
    private readonly bookFactory: IBookFactory,
    private readonly authorProvider: IAuthorProvider,
    private readonly publisherProvider: IPublisherProvider,
    private readonly publishingHouse: IPublishingHouse,
    private readonly minimumReviewersRequiredForApproval: PositiveNumber,
    private readonly maximumNumberOfTranslations: PositiveNumber,
    private readonly maxAllowedUnsoldCopiesRatioToGoOutOfPrint: Ratio,
  ) {}
}
