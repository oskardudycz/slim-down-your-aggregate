package io.eventdriven.slimdownaggregates.original;

import io.eventdriven.slimdownaggregates.original.entities.BookId;

public interface BookRepository {
  Book find(BookId bookId);
  void save(Book book);
}
