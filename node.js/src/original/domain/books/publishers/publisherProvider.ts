import { Publisher, PublisherId } from '../entities';

export interface IPublisherProvider {
  getById(publisherId: PublisherId): Promise<Publisher>;
}
