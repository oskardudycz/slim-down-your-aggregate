package io.eventdriven.slimdownaggregates.original.application.books;

import io.eventdriven.slimdownaggregates.original.domain.books.dtos.BookDetails;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;

import java.util.Optional;

public class BooksQueryServiceImpl implements BooksQueryService{
  @Override
  public Optional<BookDetails> findDetailsById(BookId bookId) {
    return Optional.empty();
  }
}
