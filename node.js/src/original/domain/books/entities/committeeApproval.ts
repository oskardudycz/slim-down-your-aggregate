import { DeepReadonly, NonEmptyString } from '#core/typing';

export type CommitteeApproval = DeepReadonly<{
  isApproved: boolean;
  feedback: NonEmptyString;
}>;
