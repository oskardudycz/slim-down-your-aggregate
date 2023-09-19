import { Book } from 'src/original/domain/books/book';
import { BookId } from 'src/original/domain/books/entities';
import { IBooksRepository } from 'src/original/domain/books/repositories';
import { IBookFactory } from 'src/original/domain/books/factories';
import { bookMapper } from '../../mappers/bookMapper';
import { PublishingHouseOrm } from '../../publishingHouseOrm';
import { BookEntity } from '..';
import { OrmRepository } from '../../core/repositories/ormRepository';

export class BooksRepository
  extends OrmRepository<Book, BookId, BookEntity, PublishingHouseOrm>
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
