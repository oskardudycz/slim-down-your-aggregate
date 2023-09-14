import { parseNonEmptyString } from '#core/typing';
import { Request } from 'express';
import { MoveToEditingCommand } from 'src/original/application/books/commands/moveToEditingCommand';
import { DeepReadonly } from 'ts-essentials';

export type MoveToEditingRequest = DeepReadonly<
  Request<Partial<{ bookId: string }>>
>;

export const toMoveToEditingCommand = (
  request: MoveToEditingRequest,
): MoveToEditingCommand => {
  const { bookId } = request.params;

  return {
    type: 'MoveToEditingCommand',
    data: {
      bookId: parseNonEmptyString(bookId),
    },
  };
};
