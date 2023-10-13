import { parseNonEmptyString } from '#core/typing';
import { Request } from 'express';
import { chapterContent } from '../../domain/books/entities';
import { DeepReadonly } from 'ts-essentials';
import { AddChapter } from '../../domain/books/draft';

export type AddChapterRequest = DeepReadonly<
  Request<
    Partial<{ bookId: string }>,
    unknown,
    Partial<{ title: string | undefined; content: string | undefined }>
  >
>;

export const toAddChapterCommand = (request: AddChapterRequest): AddChapter => {
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
