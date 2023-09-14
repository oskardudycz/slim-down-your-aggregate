import { parseNonEmptyString } from '#core/typing';
import { Request } from 'express';
import { AddChapterCommand } from 'src/original/application/books/commands/addChapterCommand';
import { chapterContent } from 'src/original/domain/books/entities';
import { DeepReadonly } from 'ts-essentials';

export type AddChapterRequest = DeepReadonly<
  Request<
    Partial<{ bookId: string }>,
    unknown,
    Partial<{ title: string | undefined; content: string | undefined }>
  >
>;

export const toAddChapterCommand = (
  request: AddChapterRequest,
): AddChapterCommand => {
  const { bookId } = request.params;
  const { title, content } = request.body;

  return {
    type: 'AddChapterCommand',
    data: {
      bookId: parseNonEmptyString(bookId),
      chapterTitle: parseNonEmptyString(title),
      chapterContent: chapterContent(content ?? ''),
    },
  };
};
