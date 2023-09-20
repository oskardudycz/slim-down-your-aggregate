import { IBooksRepository } from '../../domain/books/repositories';
import { AddChapterCommand } from './commands/addChapterCommand';
import { CreateDraftCommand } from './commands/createDraftCommand';
import { MoveToEditingCommand } from './commands/moveToEditingCommand';
import { IAuthorProvider } from '../../domain/books/authors';
import { IPublisherProvider } from '../../domain/books/publishers/publisherProvider';
import { Draft, Initial } from '../../domain/books/book';
import { InvalidOperationError, NotFoundError } from '#core/errors';

export interface IBooksService {
  createDraft(command: CreateDraftCommand): Promise<void>;
  addChapter(command: AddChapterCommand): Promise<void>;
  moveToEditing(command: MoveToEditingCommand): Promise<void>;
}

export class BooksService implements IBooksService {
  async createDraft(command: CreateDraftCommand): Promise<void> {
    const { bookId, title, author, publisherId, edition, genre } = command.data;

    const book = new Initial(bookId);

    book.createDraft(
      title,
      await this.authorProvider.getOrCreate(author),
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

    if (!(book instanceof Draft)) throw InvalidOperationError('Invalid State');

    book.addChapter(chapterTitle, chapterContent);

    return this.repository.update(book);
  }

  async moveToEditing(command: MoveToEditingCommand): Promise<void> {
    const book = await this.repository.findById(command.data.bookId);

    if (!book) throw NotFoundError("Book doesn't exist");

    if (!(book instanceof Draft)) throw InvalidOperationError('Invalid State');

    book.moveToEditing();

    return this.repository.update(book);
  }

  constructor(
    private repository: IBooksRepository,
    private authorProvider: IAuthorProvider,
    private publisherProvider: IPublisherProvider,
  ) {}
}
