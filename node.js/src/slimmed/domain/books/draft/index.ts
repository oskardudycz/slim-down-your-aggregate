import { PositiveNumber } from '#core/typing';
import { DeepReadonly } from 'ts-essentials';
import { DomainEvent } from '../../../infrastructure/events';
import {
  Genre,
  ChapterTitle,
  Chapter,
  Author,
  Publisher,
  Title,
} from '../entities';

export type Initial = DeepReadonly<{ status: 'Initial' }>;

export const initial: Initial = { status: 'Initial' };

export type Draft = DeepReadonly<{
  status: 'Draft';
  genre: Genre | null;
  chapterTitles: ChapterTitle[];
}>;

export const initialDraft: Draft = {
  status: 'Draft',
  genre: null,
  chapterTitles: [],
};

export type DraftCreated = DomainEvent<
  'DraftCreated',
  {
    title: Title;
    author: Author;
    publisher: Publisher;
    edition: PositiveNumber;
    genre: Genre | null;
  }
>;

export type ChapterAdded = DomainEvent<
  'ChapterAdded',
  {
    chapter: Chapter;
  }
>;

export type DraftEvent = DraftCreated | ChapterAdded;

export const evolve = (book: Draft, event: DraftEvent): Draft => {
  const { type, data } = event;

  switch (type) {
    case 'DraftCreated': {
      const { genre } = data;

      return { status: 'Draft', genre, chapterTitles: [] };
    }
    case 'ChapterAdded': {
      const { chapter } = data;

      return { ...book, chapterTitles: [...book.chapterTitles, chapter.title] };
    }
  }
};
export const isInitial = (obj: object): obj is Initial =>
  'status' in obj && typeof obj.status === 'string' && obj.status === 'Initial';

export const isDraft = (obj: object): obj is Draft =>
  'status' in obj && typeof obj.status === 'string' && obj.status === 'Draft';
