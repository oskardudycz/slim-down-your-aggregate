import { Request, Response } from 'express';
import { ETag, ETagErrors } from './eTag';

//////////////////////////////////////
/// ETAG
//////////////////////////////////////

export const getETagFromIfMatch = (request: Request): ETag => {
  const etag = request.headers['if-match'];

  if (etag === undefined) {
    throw ETagErrors.MISSING_IF_MATCH_HEADER;
  }

  return etag;
};

export const sendCreated = (
  response: Response,
  createdId: string,
  urlPrefix?: string,
): void => {
  response.setHeader(
    'Location',
    `${urlPrefix ?? response.req.url}/${createdId}`,
  );
  response.status(201).json({ id: createdId });
};
