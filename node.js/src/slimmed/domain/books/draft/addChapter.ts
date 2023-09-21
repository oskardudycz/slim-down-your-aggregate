import { InvalidStateError } from '#core/errors';
import { Draft } from '../book';
import { AddChapter } from '../bookCommand';
import { ChapterAdded } from '../bookEvent';
import { Chapter, chapterNumber } from '../entities';

export function addChapter(command: AddChapter, state: Draft): ChapterAdded {
  const { chapters } = state;
  const { chapterTitle, chapterContent } = command.data;

  if (chapters.some((chap) => chap.title === chapterTitle)) {
    throw InvalidStateError(
      `Chapter with title ${chapterTitle} already exists.`,
    );
  }

  if (
    chapters.length > 0 &&
    !chapters[chapters.length - 1].title.startsWith(
      `Chapter ${chapters.length}`,
    )
  ) {
    throw InvalidStateError(
      `Chapter should be added in sequence. The title of the next chapter should be 'Chapter ${
        chapters.length + 1
      }'`,
    );
  }

  const chapter = new Chapter(
    chapterNumber(chapters.length + 1),
    chapterTitle,
    chapterContent,
  );

  return {
    type: 'ChapterAdded',
    data: {
      chapter,
    },
  };
}
