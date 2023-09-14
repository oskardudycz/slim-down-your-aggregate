import { PublisherId, Publisher } from 'src/original/domain/books/entities';
import { IPublisherProvider } from 'src/original/domain/books/publishers';

export class PublisherProvider implements IPublisherProvider {
  getById(_publisherId: PublisherId): Promise<Publisher> {
    throw new Error('Method not implemented.');
  }
}
