package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record ChapterContent(String value) {
  public ChapterContent {
    assertNotNull(value);
  }

  public static final ChapterContent empty = new ChapterContent("");
}
