import { parseNonEmptyString } from '#core/typing';
import { Request } from 'express';
import { FindDetailsByIdQuery } from 'src/original/application/books/queries';
import { DeepReadonly } from 'ts-essentials';

export type FindDetailsByIdRequest = DeepReadonly<
  Request<Partial<{ bookId: string }>>
>;

export const toFindDetailsByIdQuery = (
  request: FindDetailsByIdRequest,
): FindDetailsByIdQuery => {
  const { bookId } = request.params;

  return {
    type: 'FindDetailsByIdQuery',
    data: {
      bookId: parseNonEmptyString(bookId),
    },
  };
};
