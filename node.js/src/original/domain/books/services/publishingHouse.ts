import { Genre } from '../entities';

export interface IPublishingHouse {
  isGenreLimitReached(genre: Genre): boolean;
}
