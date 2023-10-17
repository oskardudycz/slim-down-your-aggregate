package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record Publisher(PublisherId id, PublisherName name) {
  public Publisher {
    assertNotNull(id);
    assertNotNull(name);
  }
}
