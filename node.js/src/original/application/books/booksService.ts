import { IBooksRepository } from 'src/original/domain/books/repositories';
import { AddChapterCommand } from './commands/addChapterCommand';
import { CreateDraftCommand } from './commands/createDraftCommand';
import { MoveToEditingCommand } from './commands/moveToEditingCommand';
import { IAuthorProvider } from 'src/original/domain/books/authors';
import { IPublisherProvider } from 'src/original/domain/books/publishers/publisherProvider';
import { Book } from 'src/original/domain/books/book';
import { IPublishingHouse } from 'src/original/domain/books/services';

export interface IBooksService {
  createDraft(command: CreateDraftCommand): Promise<void>;
  addChapter(command: AddChapterCommand): Promise<void>;
  moveToEditing(command: MoveToEditingCommand): Promise<void>;
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

    // TODO: Add Explicit Not Found exception
    if (!book) throw new Error("Book doesn't exist");

    book.addChapter(chapterTitle, chapterContent);

    return this.repository.update(book);
  }

  async moveToEditing(command: MoveToEditingCommand): Promise<void> {
    const book = await this.repository.findById(command.data.bookId);

    // TODO: Add Explicit Not Found exception
    if (!book) throw new Error("Book doesn't exist");

    book.moveToEditing();

    return this.repository.update(book);
  }

  constructor(
    private repository: IBooksRepository,
    private authorProvider: IAuthorProvider,
    private publisherProvider: IPublisherProvider,
  ) {}
}