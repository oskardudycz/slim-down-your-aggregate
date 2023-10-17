package io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotEmpty;

public record NonEmptyString(String value) {
  public NonEmptyString {
    assertNotEmpty(value);
  }
}
