package io.eventdriven.slimdownaggregates.shorter.original.entities;

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
