import { PositiveNumber } from '#core/typing';
import { DraftCreated } from '../draft';
import { BookId, Title, Author, Publisher, Genre } from '../entities';

export class Initial {
  public get id() {
    return this._id;
  }

  constructor(private readonly _id: BookId) {}

  createDraft(
    title: Title,
    author: Author,
    publisher: Publisher,
    edition: PositiveNumber,
    genre: Genre | null,
  ): DraftCreated {
    return {
      type: 'DraftCreated',
      data: {
        bookId: this.id,
        title,
        author,
        publisher,
        edition,
        genre,
      },
    };
  }
}
