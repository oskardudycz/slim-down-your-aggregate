import { Book } from '../../domain/books/book';
import { BookEntity } from '../books/bookEntity';
import { IBookFactory } from '../../domain/books/factories';
import { nonEmptyString, nonEmptyUuid, positiveNumber } from '#core/typing';
import { Chapter, chapterContent, Format } from '../../domain/books/entities';

export const bookMapper = {
  mapFromEntity: (entity: BookEntity, bookFactory: IBookFactory): Book => {
    return bookFactory.create(
      nonEmptyString(entity.id),
      entity.currentState,
      nonEmptyString(entity.title),
      {
        authorId: nonEmptyString(entity.author.id),
        firstName: nonEmptyString(entity.author.firstName),
        lastName: nonEmptyString(entity.author.lastName),
      },
      entity.genre ? nonEmptyString(entity.genre) : null,
      entity.isbn ? nonEmptyString(entity.isbn) : null,
      entity.committeeApproval
        ? {
            feedback: nonEmptyString(entity.committeeApproval.feedback),
            isApproved: entity.committeeApproval.isApproved,
          }
        : null,
      entity.reviewers.map((r) => {
        return { id: nonEmptyUuid(r.id), name: nonEmptyString(r.name) };
      }),
      entity.chapters.map((ch) => {
        return new Chapter(
          positiveNumber(ch.number),
          nonEmptyString(ch.title),
          chapterContent(ch.content),
        );
      }),
      entity.translations.map((t) => {
        return {
          language: {
            id: nonEmptyUuid(t.language.id),
            name: nonEmptyString(t.language.name),
          },
          translator: {
            id: nonEmptyUuid(t.translator.id),
            name: nonEmptyString(t.translator.name),
          },
        };
      }),
      entity.formats.map((f) => {
        return new Format(
          nonEmptyString(f.formatType),
          positiveNumber(f.soldCopies),
          positiveNumber(f.totalCopies),
        );
      }),
    );
  },
  mapToEntity: (book: Book): BookEntity => {
    return {
      id: book.id,
      currentState: book.currentState,
      title: book.title,
      author: {
        id: book.author.authorId,
        firstName: book.author.firstName,
        lastName: book.author.lastName,
      },
      publisher: { id: book.publisher.id, name: book.publisher.name },
      edition: book.edition,
      genre: book.genre,
      isbn: book.isbn,
      publicationDate: book.publicationDate,
      totalPages: book.totalPages,
      numberOfIllustrations: book.numberOfIllustrations,
      bindingType: book.bindingType,
      summary: book.summary,
      committeeApproval: book.committeeApproval
        ? {
            feedback: book.committeeApproval.feedback,
            isApproved: book.committeeApproval.isApproved,
          }
        : null,
      reviewers: book.reviewers.map((r) => {
        return { id: r.id, name: r.name };
      }),
      chapters: book.chapters.map((ch) => {
        return { number: ch.number, title: ch.title, content: ch.content };
      }),
      translations: book.translations.map((t) => {
        return {
          language: { id: t.language.id, name: t.language.name },
          translator: { id: t.translator.id, name: t.translator.name },
        };
      }),
      formats: book.formats.map((f) => {
        return {
          formatType: f.formatType,
          soldCopies: f.soldCopies,
          totalCopies: f.totalCopies,
        };
      }),
    };
  },
};
