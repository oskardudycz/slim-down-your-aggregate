import { BookId } from '../../../domain/books/entities';
import { PublishingHouseOrm } from '../../publishingHouseOrm';
import { BookEntity, State } from '..';
import { OrmRepository } from '../../core/repositories/ormRepository';
import { BookEvent, PublishedExternal } from '../../../domain/books/book';
import { NotFoundError } from '#core/errors';
import { EventEnvelope } from '../../../infrastructure/events';
import { nonEmptyString } from '#core/typing';

export interface IBooksRepository {
  getAndUpdate(
    key: BookId,
    handle: (entity: BookEntity | null) => BookEvent[],
  ): Promise<void>;
}

export class BooksRepository
  extends OrmRepository<BookEntity, BookId, BookEvent, PublishingHouseOrm>
  implements IBooksRepository
{
  constructor(orm: PublishingHouseOrm) {
    super(orm, orm.books);
  }

  protected evolve(
    orm: PublishingHouseOrm,
    current: BookEntity | null,
    eventEnvelope: EventEnvelope<BookEvent>,
  ): void {
    const {
      metadata: { recordId: id },
    } = eventEnvelope;
    const { type, data } = eventEnvelope.event;

    switch (type) {
      case 'DraftCreated': {
        orm.books.add(id, {
          id,
          currentState: State.Writing,
          title: data.title,
          author: {
            id: data.author.authorId,
            firstName: data.author.firstName,
            lastName: data.author.lastName,
          },
          publisher: { id: data.publisher.id, name: data.publisher.name },
          edition: data.edition,
          genre: data.genre,
          chapters: [],
          formats: [],
          reviewers: [],
          translations: [],
          version: 0,
        });
        break;
      }
      case 'ChapterAdded': {
        if (!current) throw NotFoundError('Book was not found');

        const { number, title, content } = data.chapter;

        const chapter = {
          bookId: id,
          number,
          title,
          content,
        };

        orm.books.patch(id, {
          version: current.version + 1,
          // line below is needed as my dummy orm is indeed dummy
          // normally we have relational db joins
          chapters: [...current.chapters, chapter],
        });
        orm.bookChapters.add(`${current.id}_${number}`, chapter);

        break;
      }
      case 'FormatAdded': {
        if (!current) throw NotFoundError('Book was not found');

        const {
          format: { formatType, soldCopies, totalCopies },
        } = data;

        const format = {
          bookId: current.id,
          formatType,
          soldCopies,
          totalCopies,
        };

        orm.books.patch(current.id, {
          version: current.version + 1,
          // line below is needed as my dummy orm is indeed dummy
          // normally we have relational db joins
          formats: [...current.formats, format],
        });
        orm.bookFormats.add(`${current.id}_${formatType}`, format);

        break;
      }
      case 'FormatRemoved': {
        if (!current) throw NotFoundError('Book was not found');

        const {
          format: { formatType },
        } = data;

        orm.books.patch(current.id, { version: current.version + 1 });
        orm.bookFormats.delete(`${current.id}_${formatType}`);

        break;
      }
      case 'TranslationAdded': {
        if (!current) throw NotFoundError('Book was not found');

        const {
          translation: { language, translator },
        } = data;

        const translation = {
          bookId: current.id,
          language,
          translator,
        };
        orm.books.patch(current.id, {
          version: current.version + 1,
          // line below is needed as my dummy orm is indeed dummy
          // normally we have relational db joins
          translations: [...current.translations, translation],
        });
        orm.bookTranslations.add(
          `${current.id}_${language.id}_${translator.id}`,
          translation,
        );

        break;
      }
      case 'TranslationRemoved': {
        if (!current) throw NotFoundError('Book was not found');

        const {
          translation: {
            language: { id: languageId },
            translator: { id: translatorId },
          },
        } = data;

        orm.books.patch(current.id, { version: current.version + 1 });
        orm.bookTranslations.delete(
          `${current.id}_${languageId}_${translatorId}`,
        );

        break;
      }
      case 'ReviewerAdded': {
        if (!current) throw NotFoundError('Book was not found');

        const { reviewer } = data;

        orm.books.patch(current.id, {
          version: current.version + 1,
          // line below is needed as my dummy orm is indeed dummy
          // normally we have relational db joins
          reviewers: [...current.reviewers, reviewer],
        });
        orm.bookReviewers.add(`${current.id}_${reviewer.id}`, {
          bookId: current.id,
          reviewer,
        });

        break;
      }
      case 'MovedToEditing': {
        if (!current) throw NotFoundError('Book was not found');

        orm.books.patch(current.id, {
          currentState: State.Editing,
          version: current.version + 1,
        });

        break;
      }
      case 'Approved': {
        if (!current) throw NotFoundError('Book was not found');

        orm.books.patch(current.id, {
          committeeApproval: data.committeeApproval,
          version: current.version + 1,
        });

        break;
      }
      case 'ISBNSet': {
        if (!current) throw NotFoundError('Book was not found');

        orm.books.patch(current.id, {
          isbn: data.isbn,
          version: current.version + 1,
        });

        break;
      }
      case 'MovedToPrinting': {
        if (!current) throw NotFoundError('Book was not found');

        orm.books.patch(current.id, {
          currentState: State.Printing,
          version: current.version + 1,
        });

        break;
      }
      case 'Published': {
        if (!current) throw NotFoundError('Book was not found');

        orm.books.patch(current.id, {
          currentState: State.Published,
          version: current.version + 1,
        });

        break;
      }
      case 'MovedToOutOfPrint': {
        if (!current) throw NotFoundError('Book was not found');

        orm.books.patch(current.id, {
          currentState: State.OutOfPrint,
          version: current.version + 1,
        });

        break;
      }
    }
  }

  protected enrich(
    eventEnvelope: EventEnvelope<BookEvent>,
    current: BookEntity | null,
  ): EventEnvelope {
    const { type } = eventEnvelope.event;

    // we can enrich events published externally to outbox using data from events and entity
    if (type == 'Published' && current !== null) {
      const external: PublishedExternal = {
        type: 'Published',
        data: {
          bookId: nonEmptyString(current.id),
          isbn: nonEmptyString(current.isbn!),
          title: nonEmptyString(current.title),
          authorId: nonEmptyString(current.author.id),
        },
      };

      return { event: external, metadata: eventEnvelope.metadata };
    }

    return super.enrich(eventEnvelope, current);
  }
}
