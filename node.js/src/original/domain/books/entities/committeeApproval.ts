import { NonEmptyString } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';

export type CommitteeApproval = DeepReadonly<{
  isApproved: boolean;
  feedback: NonEmptyString;
}>;
