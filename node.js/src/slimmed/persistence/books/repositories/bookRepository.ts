import { Book } from '../../../domain/books/book';
import { BookId } from '../../../domain/books/entities';
import { IBooksRepository } from '../../../domain/books/repositories';
import { IBookFactory } from '../../../domain/books/factories';
import { bookMapper } from '../../mappers/bookMapper';
import { PublishingHouseOrm } from '../../publishingHouseOrm';
import { BookEntity } from '..';
import { OrmRepository } from '../../core/repositories/ormRepository';
import { BookEvent } from 'src/slimmed/domain/books/bookEvent';

export class BooksRepository
  extends OrmRepository<Book, BookId, BookEvent, BookEntity, PublishingHouseOrm>
  implements IBooksRepository
{
  constructor(
    orm: PublishingHouseOrm,
    private bookFactory: IBookFactory,
  ) {
    super(orm, orm.books);
  }

  protected mapToAggregate = (entity: BookEntity): Book =>
    bookMapper.mapFromEntity(entity, this.bookFactory);

  protected mapToEntity = (aggregate: Book): BookEntity =>
    bookMapper.mapToEntity(aggregate);
}
