package io.eventdriven.slimdownaggregates.slimmed.entities;

public class ISBN {
  private final String number;

  public ISBN(String number) {
    this.number = number;
  }

  public String getNumber() {
    return number;
  }
}
