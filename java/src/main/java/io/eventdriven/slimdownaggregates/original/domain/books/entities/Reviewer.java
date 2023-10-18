package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record Reviewer(ReviewerId id, ReviewerName name) {
  public Reviewer {
    assertNotNull(id);
    assertNotNull(name);
  }
}
