package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record Reviewer(PublisherId id, PublisherName name) {
  public Reviewer {
    assertNotNull(id);
    assertNotNull(name);
  }
}
