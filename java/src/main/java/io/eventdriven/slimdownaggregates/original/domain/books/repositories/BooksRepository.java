package io.eventdriven.slimdownaggregates.original.domain.books.repositories;

import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;

import java.util.Optional;

public interface BooksRepository {
  Optional<Book> findById(BookId bookId);

  void add(Book book);

  void update(Book book);
}
