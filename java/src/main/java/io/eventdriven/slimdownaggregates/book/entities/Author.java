package io.eventdriven.slimdownaggregates.book.entities;

public class Author {
  // Assuming that the Author class has a name
  private final String name;

  public Author(String name) {
    this.name = name;
  }

  public String getName() {
    return name;
  }
}
