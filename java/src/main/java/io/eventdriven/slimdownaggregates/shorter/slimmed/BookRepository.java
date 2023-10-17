package io.eventdriven.slimdownaggregates.shorter.slimmed;

import io.eventdriven.slimdownaggregates.shorter.slimmed.entities.BookId;

public interface BookRepository {
  Book find(BookId bookId);
  void save(BookEvent bookEvent);
}
