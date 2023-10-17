package io.eventdriven.slimdownaggregates.shorter.original;

import io.eventdriven.slimdownaggregates.shorter.original.entities.BookId;

public interface BookRepository {
  Book find(BookId bookId);
  void save(Book book);
}
