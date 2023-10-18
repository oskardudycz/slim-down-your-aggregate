package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import java.util.UUID;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record PublisherId(UUID value) {
  public PublisherId {
    assertNotNull(value);
  }
}
