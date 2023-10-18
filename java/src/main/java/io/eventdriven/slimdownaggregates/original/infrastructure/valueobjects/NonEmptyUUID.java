package io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects;

import java.util.UUID;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record NonEmptyUUID(UUID value) {
  public NonEmptyUUID {
    assertNotNull(value);
  }
}
