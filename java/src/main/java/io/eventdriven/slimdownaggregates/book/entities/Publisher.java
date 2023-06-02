package io.eventdriven.slimdownaggregates.book.entities;

public class Publisher {
  private final String name;

  public Publisher(String name) {
    this.name = name;
  }

  public String getName() {
    return name;
  }
}
