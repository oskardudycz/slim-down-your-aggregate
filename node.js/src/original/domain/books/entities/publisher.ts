import { DeepReadonly, NonEmptyString, NonEmptyUuid } from '#core/typing';

export type PublisherId = NonEmptyUuid<'PublisherId'>;
export type PublisherName = NonEmptyString<'PublisherName'>;

export type Publisher = DeepReadonly<{ Id: PublisherId; Name: PublisherName }>;
