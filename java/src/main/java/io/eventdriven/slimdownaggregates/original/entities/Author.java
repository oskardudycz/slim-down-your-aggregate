package io.eventdriven.slimdownaggregates.original.entities;

public class Author {
  // Assuming that the author class has a name
  private final String name;

  public Author(String name) {
    this.name = name;
  }

  public String getName() {
    return name;
  }
}
