import { ApplicationErrorType, isApplicationError } from '#core/errors';
import { NextFunction, Request, Response } from 'express';
import { ProblemDocument } from 'http-problem-details';

export const problemDetailsMiddleware = (
  error: Error,
  _request: Request,
  response: Response,
  _next: NextFunction,
): void => {
  let statusCode = 500;
  let title = undefined;
  const type = undefined;

  if (isApplicationError(error)) {
    title = `${error.type} error`;
    switch (error.type) {
      case ApplicationErrorType.Validation: {
        statusCode = 400;
        break;
      }
      case ApplicationErrorType.InvalidOperation: {
        statusCode = 403;
        break;
      }
      case ApplicationErrorType.NotFound: {
        statusCode = 404;
        break;
      }
      case ApplicationErrorType.InvalidState: {
        statusCode = 412;
        break;
      }
    }
  }
  response.statusCode = statusCode;
  response.setHeader('Content-Type', 'application/problem+json');
  response.json(
    new ProblemDocument({
      detail: error.message,
      title,
      status: statusCode,
      type,
    }),
  );
};
