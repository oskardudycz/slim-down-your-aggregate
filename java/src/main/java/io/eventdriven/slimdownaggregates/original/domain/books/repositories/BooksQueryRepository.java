package io.eventdriven.slimdownaggregates.original.domain.books.repositories;

import io.eventdriven.slimdownaggregates.original.domain.books.dtos.BookDetails;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;

import java.util.Optional;

public interface BooksQueryRepository {
  Optional<BookDetails> findDetailsById(BookId bookId);
}
