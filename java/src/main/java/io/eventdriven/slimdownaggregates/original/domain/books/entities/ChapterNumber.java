package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.*;

public record ChapterNumber(int value) {
  public ChapterNumber {
    assertPositive(value);
  }
}
