//import { BookId } from '#original';
import {
  BookId,
  ChapterContent,
  ChapterTitle,
} from '../../../domain/books/entities';
import { Command } from '../../../infrastructure/commands';

export type AddChapterCommand = Command<
  'AddChapterCommand',
  {
    bookId: BookId;
    chapterTitle: ChapterTitle;
    chapterContent: ChapterContent;
  }
>;
