import { Command } from 'src/slimmed/infrastructure/commands';
import { Draft } from '../book';
import { BookEvent } from '../bookEvent';
import { addChapter } from './addChapter';
import { moveToEditing } from './moveToEditing';
import { BookId, ChapterTitle, ChapterContent } from '../entities';

export type AddChapter = Command<
  'AddChapter',
  {
    bookId: BookId;
    chapterTitle: ChapterTitle;
    chapterContent: ChapterContent;
  }
>;

export type MoveToEditing = Command<
  'MoveToEditing',
  {
    bookId: BookId;
  }
>;

export type DraftCommand = AddChapter | MoveToEditing;

export function decideChapter(
  command: AddChapter | MoveToEditing,
  state: Draft,
): BookEvent[] {
  switch (command.type) {
    case 'AddChapter':
      return [addChapter(command, state)];
    case 'MoveToEditing':
      return [moveToEditing(command, state)];
  }
}
