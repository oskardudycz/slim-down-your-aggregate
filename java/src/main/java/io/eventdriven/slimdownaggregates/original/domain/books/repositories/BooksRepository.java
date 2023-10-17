package io.eventdriven.slimdownaggregates.original.domain.books.repositories;

import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;

import java.util.Optional;

public interface BooksRepository {
  Optional<Book> FindById(BookId bookId);

  void Add(Book book);

  void Update(Book book);
}
