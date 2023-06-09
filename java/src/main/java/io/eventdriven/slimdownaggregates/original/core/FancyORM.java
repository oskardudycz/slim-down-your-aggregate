package io.eventdriven.slimdownaggregates.original.core;

public interface FancyORM {
  <T> T find(Object id);
  <T> void store(T book);
  void save();
}
