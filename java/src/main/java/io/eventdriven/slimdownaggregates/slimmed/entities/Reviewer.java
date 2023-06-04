package io.eventdriven.slimdownaggregates.slimmed.entities;

public class Reviewer {
  private final String name;

  public Reviewer(String name) {
    this.name = name;
  }

  public String getName() {
    return name;
  }
}
