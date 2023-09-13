//import { BookId } from '#original';
import {
  BookId,
  ChapterContent,
  ChapterTitle,
} from 'src/original/domain/books/entities';
import { Command } from 'src/original/infrastructure/commands';

export type AddChapterCommand = Command<
  'AddChapterCommand',
  {
    bookId: BookId;
    chapterTitle: ChapterTitle;
    chapterContent: ChapterContent;
  }
>;
