package io.eventdriven.slimdownaggregates.slimmed.entities;

import java.util.UUID;

public class BookId {
  private final UUID value;

  public BookId(UUID value) {
    this.value = value;
  }

  public UUID getValue() {
    return value;
  }
}
