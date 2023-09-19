import { BookDetails } from '../../../domain/books/dtos';
import { BookId } from '../../../domain/books/entities';
import { IBooksQueryRepository } from '../../../domain/books/repositories';
import { PublishingHouseOrm } from '../../publishingHouseOrm';

export class BookQueryRepository implements IBooksQueryRepository {
  constructor(private orm: PublishingHouseOrm) {}

  async findDetailsById(bookId: BookId): Promise<BookDetails | null> {
    const entity = await this.orm.books.findById(bookId);

    if (entity === null) return null;

    return {
      id: entity.id,
      currentState: entity.currentState,
      title: entity.title,
      author: {
        firstName: entity.author.firstName,
        lastName: entity.author.lastName,
      },
      publisherName: entity.publisher.name,
      edition: entity.edition,
      genre: entity.genre,
      isbn: entity.isbn,
      publicationDate: entity.publicationDate?.toString(),
      totalPages: entity.totalPages,
      numberOfIllustrations: entity.numberOfIllustrations,
      bindingType: entity.bindingType,
      summary: entity.summary,
      committeeApproval: entity.committeeApproval
        ? {
            feedback: entity.committeeApproval.feedback,
            isApproved: entity.committeeApproval.isApproved,
          }
        : null,
      reviewers: entity.reviewers.map((r) => {
        return r.name;
      }),
      chapters: entity.chapters.map((ch) => {
        return { number: ch.number, title: ch.title, content: ch.content };
      }),
      translations: entity.translations.map((t) => {
        return { language: t.language.name, translator: t.translator.name };
      }),
      formats: entity.formats.map((f) => {
        return {
          formatType: f.formatType,
          soldCopies: f.soldCopies,
          totalCopies: f.totalCopies,
        };
      }),
    };
  }
}
