package io.eventdriven.slimdownaggregates.slimmed.entities;

public class Genre {
  private final String name;

  public Genre(String name) {
    this.name = name;
  }

  public String getName() {
    return name;
  }
}
