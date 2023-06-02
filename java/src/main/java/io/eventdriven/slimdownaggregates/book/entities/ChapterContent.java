package io.eventdriven.slimdownaggregates.book.entities;

public class ChapterContent {
  private final String value;

  public ChapterContent(String value) {
    this.value = value;
  }

  public String getValue() {
    return value;
  }
}
