import { parseNonEmptyString } from '#core/typing';
import { Request } from 'express';
import { MoveToEditing } from '../../domain/books/draft';
import { DeepReadonly } from 'ts-essentials';

export type MoveToEditingRequest = DeepReadonly<
  Request<Partial<{ bookId: string }>>
>;

export const toMoveToEditingCommand = (
  request: MoveToEditingRequest,
): MoveToEditing => {
  const { bookId } = request.params;

  return {
    type: 'MoveToEditingCommand',
    data: {
      bookId: parseNonEmptyString(bookId),
    },
  };
};
