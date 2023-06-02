package io.eventdriven.slimdownaggregates.book.entities;

public class ChapterTitle {
  private final String value;

  public ChapterTitle(String value) {
    this.value = value;
  }

  public String getValue() {
    return value;
  }
}
