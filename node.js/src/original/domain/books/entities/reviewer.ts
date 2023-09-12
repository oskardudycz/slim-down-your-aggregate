import { DeepReadonly, NonEmptyString, NonEmptyUuid } from '#core/typing';

export type ReviewerId = NonEmptyUuid<'ReviewerId'>;
export type ReviewerName = NonEmptyString<'ReviewerName'>;

export type Reviewer = DeepReadonly<{ Id: ReviewerId; Name: ReviewerName }>;
