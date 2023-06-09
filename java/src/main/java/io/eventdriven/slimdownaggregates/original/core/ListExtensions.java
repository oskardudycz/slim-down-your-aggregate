package io.eventdriven.slimdownaggregates.original.core;

import java.util.ArrayList;
import java.util.List;
import java.util.function.Predicate;

public final class ListExtensions {
  public static <T> List<T> union(List<T> list, T element) {

    var newList = new ArrayList<>(list);
    newList.add(element);

    return newList;
  }

  public static <T> List<T> except(List<T> list, Predicate<T> predicate) {
    return list.stream().filter(predicate.negate()).toList();
  }
}
