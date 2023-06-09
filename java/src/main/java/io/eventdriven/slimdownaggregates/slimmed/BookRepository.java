package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.BookId;

public interface BookRepository {
  Book find(BookId bookId);
  void save(BookEvent bookEvent);
}
