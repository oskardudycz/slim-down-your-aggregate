package io.eventdriven.slimdownaggregates.book.entities;

public class Chapter {
  private final ChapterTitle title;
  private final ChapterContent content;

  public Chapter(ChapterTitle title, ChapterContent content) {
    this.title = title;
    this.content = content;
  }

  public ChapterTitle getTitle() {
    return title;
  }

  public ChapterContent getContent() {
    return content;
  }
}
