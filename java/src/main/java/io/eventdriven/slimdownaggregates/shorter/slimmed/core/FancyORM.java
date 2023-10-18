package io.eventdriven.slimdownaggregates.shorter.slimmed.core;

public interface FancyORM {
  <T> T find(Object id);
  <T> void store(T book);
  void save();
}
