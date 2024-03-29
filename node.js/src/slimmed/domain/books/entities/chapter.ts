import {
  Brand,
  NonEmptyString,
  PositiveNumber,
  positiveNumber,
} from '#core/typing';

export type ChapterNumber = PositiveNumber<'ChapterNumber'>;
export const chapterNumber = (number: number): ChapterNumber =>
  positiveNumber(number);

export type ChapterTitle = NonEmptyString<'ChapterTitle'>;

export type ChapterContent = Brand<string, 'ChapterContent'>;

export const chapterContent = (content: string) => {
  return content as ChapterContent;
};

export class Chapter {
  readonly #number: ChapterNumber;
  #title: ChapterTitle;
  #content: ChapterContent;

  public get number() {
    return this.#number;
  }
  public get title() {
    return this.#title;
  }
  public get content() {
    return this.#content;
  }

  constructor(
    number: ChapterNumber,
    title: ChapterTitle,
    content: ChapterContent,
  ) {
    this.#number = number;
    this.#title = title;
    this.#content = content;
  }

  public changeTitle(title: ChapterTitle) {
    this.#title = title;
  }

  public changeContent(content: ChapterContent) {
    this.#content = content;
  }
}
