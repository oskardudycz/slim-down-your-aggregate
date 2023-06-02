package io.eventdriven.slimdownaggregates.book.entities;

public class Title {
  private final String value;

  public Title(String value) {
    this.value = value;
  }

  public String getValue() {
    return value;
  }
}
