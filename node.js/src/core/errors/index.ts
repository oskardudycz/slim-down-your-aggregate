export enum ApplicationErrorType {
  Validation = 'Validation',
  InvalidOperation = 'InvalidOperation',
  PreconditionFailed = 'PreconditionFailed',
  NotFound = 'NotFound',
  Unknown = 'Unknown',
}

export type ApplicationError = Error & { type: ApplicationErrorType };

export const ApplicationError = (
  type: ApplicationErrorType,
  errorOrMessage: Error | string,
) => {
  const error =
    typeof errorOrMessage !== 'string'
      ? errorOrMessage
      : new Error(errorOrMessage);
  return { type, ...error };
};

export const ValidationError = (message: string) =>
  ApplicationError(ApplicationErrorType.Validation, message);

export const InvalidOperationError = (errorOrMessage: Error | string) =>
  ApplicationError(ApplicationErrorType.InvalidOperation, errorOrMessage);

export const NotFoundError = (message: string) =>
  ApplicationError(ApplicationErrorType.NotFound, message);

export const InvalidStateError = (message: string) =>
  ApplicationError(ApplicationErrorType.PreconditionFailed, message);

export const UnknownError = (errorOrMessage: Error | string) =>
  ApplicationError(ApplicationErrorType.Unknown, errorOrMessage);

export const isApplicationError = (error: Error): error is ApplicationError => {
  return (
    'type' in error &&
    typeof error.type === 'string' &&
    (Object.values(ApplicationErrorType) as string[]).includes(error.type)
  );
};
