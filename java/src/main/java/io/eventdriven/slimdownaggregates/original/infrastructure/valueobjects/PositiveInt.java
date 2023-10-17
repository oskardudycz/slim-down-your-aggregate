package io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertPositive;

public record PositiveInt(int value) {
  public PositiveInt {
    assertPositive(value);
  }
}
