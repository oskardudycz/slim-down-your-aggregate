import { PublisherId, Publisher } from '../../domain/books/entities';
import { IPublisherProvider } from '../../domain/books/publishers';
import { nonEmptyString, nonEmptyUuid } from '#core/typing';
import { PublishingHouseOrm } from '../publishingHouseOrm';

export class PublisherProvider implements IPublisherProvider {
  constructor(private orm: PublishingHouseOrm) {}

  async getById(publisherId: PublisherId): Promise<Publisher> {
    const entity = await this.orm.publishers.findById(publisherId);

    if (!entity) throw Error(`Publisher with id ${publisherId} not found`);

    const { id, name } = entity;

    return {
      id: nonEmptyUuid(id),
      name: nonEmptyString(name),
    };
  }
}
