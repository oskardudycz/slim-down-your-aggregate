package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotEmpty;

public record AuthorFirstName(String value) {
  public AuthorFirstName {
    assertNotEmpty(value);
  }
}
