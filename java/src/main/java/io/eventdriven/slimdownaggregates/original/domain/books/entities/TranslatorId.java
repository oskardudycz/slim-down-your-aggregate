package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import java.util.UUID;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record TranslatorId(UUID value) {
  public TranslatorId {
    assertNotNull(value);
  }
}
