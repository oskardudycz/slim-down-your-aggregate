package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotEmpty;

public record Genre(String value) {
  public Genre {
    assertNotEmpty(value);
  }
}
