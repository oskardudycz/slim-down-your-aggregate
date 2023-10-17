package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.*;

public record ChapterTitle(String value) {
  public ChapterTitle {
    assertNotEmpty(value);
  }
}
