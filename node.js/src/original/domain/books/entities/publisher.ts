import { NonEmptyString, NonEmptyUuid } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type PublisherId = NonEmptyUuid<'PublisherId'>;
export type PublisherName = NonEmptyString<'PublisherName'>;

export type Publisher = DeepReadonly<{ id: PublisherId; name: PublisherName }>;
