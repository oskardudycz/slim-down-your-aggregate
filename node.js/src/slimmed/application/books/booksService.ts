import { IBooksRepository } from '../../domain/books/repositories';
import { AddChapterCommand } from './commands/addChapterCommand';
import { CreateDraftCommand } from './commands/createDraftCommand';
import { MoveToEditingCommand } from './commands/moveToEditingCommand';
import { IAuthorProvider } from '../../domain/books/authors';
import { IPublisherProvider } from '../../domain/books/publishers/publisherProvider';
import { Draft, Initial } from '../../domain/books/book';
import { InvalidOperationError, NotFoundError } from '#core/errors';
import { commandHandler } from 'src/slimmed/domain/books/commandHandler';
import {
  AddChapter,
  CreateDraft,
  MoveToEditing,
  bookCommandHandler,
} from 'src/slimmed/domain/books/bookCommand';

export interface IBooksService {
  createDraft(command: CreateDraft): Promise<void>;
  addChapter(command: AddChapter): Promise<void>;
  moveToEditing(command: MoveToEditing): Promise<void>;
}

export class BooksService implements IBooksService {
  createDraft(command: CreateDraft): Promise<void> {
    return bookCommandHandler(this.repository, command.data.bookId, [command]);
  }

  async addChapter(command: AddChapter): Promise<void> {
    return bookCommandHandler(this.repository, command.data.bookId, [command]);
  }

  async moveToEditing(command: MoveToEditing): Promise<void> {
    return bookCommandHandler(this.repository, command.data.bookId, [command]);
  }

  constructor(
    private repository: IBooksRepository,
    private authorProvider: IAuthorProvider,
    private publisherProvider: IPublisherProvider,
  ) {}
}
