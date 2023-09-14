import { PublisherId, Publisher } from 'src/original/domain/books/entities';
import { IPublisherProvider } from 'src/original/domain/books/publishers';
import { ORM } from '../orm';
import { nonEmptyString, nonEmptyUuid } from '#core/typing';

export class PublisherProvider implements IPublisherProvider {
  constructor(private orm: ORM) {}

  async getById(publisherId: PublisherId): Promise<Publisher> {
    const entity = await this.orm.publishers.get(publisherId);

    if (!entity) throw Error(`Publisher with id ${publisherId} not found`);

    const { id, name } = entity;

    return {
      id: nonEmptyUuid(id),
      name: nonEmptyString(name),
    };
  }
}
