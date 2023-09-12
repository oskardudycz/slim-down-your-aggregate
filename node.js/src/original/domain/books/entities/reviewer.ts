import { NonEmptyString, NonEmptyUuid } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type ReviewerId = NonEmptyUuid<'ReviewerId'>;
export type ReviewerName = NonEmptyString<'ReviewerName'>;

export type Reviewer = DeepReadonly<{ id: ReviewerId; name: ReviewerName }>;
