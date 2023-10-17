package io.eventdriven.slimdownaggregates.original.infrastructure.validation;

public final class Validation {
  public static void assertNotNull(Object obj) {
    if(obj == null)
      throw new IllegalArgumentException("Text cannot be empty");
  }

  public static void assertPositive(int number) {
    if(number <= 0)
      throw new IllegalArgumentException("Number has to be positive");
  }

  public static void assertNotEmpty(String text) {
    assertNotNull(text);

    if(text.isEmpty())
      throw new IllegalArgumentException("Text cannot be empty");
  }
}
