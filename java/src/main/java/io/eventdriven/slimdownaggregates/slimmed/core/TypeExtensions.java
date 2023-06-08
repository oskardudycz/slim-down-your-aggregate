package io.eventdriven.slimdownaggregates.slimmed.core;

public final class TypeExtensions {
  public static <T> T ofType(Object obj, Class<T> type) {
    if (!type.isInstance(obj))
      throw new IllegalStateException();

    return type.cast(obj);
  }
}
