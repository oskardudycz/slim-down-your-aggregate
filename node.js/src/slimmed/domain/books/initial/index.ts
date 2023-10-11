import { PositiveNumber } from '#core/typing';
import { DraftCreated } from '../draft';
import { Title, Author, Publisher, Genre } from '../entities';

export class Initial {
  static createDraft(
    _initial: Initial,
    title: Title,
    author: Author,
    publisher: Publisher,
    edition: PositiveNumber,
    genre: Genre | null,
  ): DraftCreated {
    return {
      type: 'DraftCreated',
      data: {
        title,
        author,
        publisher,
        edition,
        genre,
      },
    };
  }
}