package io.eventdriven.slimdownaggregates.original.domain.books.entities;

public class Chapter {
  private final ChapterNumber chapterNumber;
  private ChapterTitle title;
  private ChapterContent content;

  public Chapter(
    ChapterNumber chapterNumber,
    ChapterTitle title,
    ChapterContent content
  ) {
    this.chapterNumber = chapterNumber;
    this.title = title;
    this.content = content;
  }

  public ChapterNumber chapterNumber() {
    return chapterNumber;
  }

  public ChapterTitle title() {
    return title;
  }

  public ChapterContent content() {
    return content;
  }

  public void changeTitle(ChapterTitle title) {
    this.title = title;
  }

  public void changeContent(ChapterContent content) {
    this.content = content;
  }
}
