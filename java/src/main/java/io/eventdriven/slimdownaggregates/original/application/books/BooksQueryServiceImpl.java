package io.eventdriven.slimdownaggregates.original.application.books;

import io.eventdriven.slimdownaggregates.original.domain.books.dtos.BookDetails;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;
import io.eventdriven.slimdownaggregates.original.domain.books.repositories.BooksQueryRepository;

import java.util.Optional;

public class BooksQueryServiceImpl implements BooksQueryService{
  @Override
  public Optional<BookDetails> findDetailsById(BookId bookId) {
    return repository.findDetailsById(bookId);
  }

  public BooksQueryServiceImpl(BooksQueryRepository repository) {
    this.repository = repository;
  }

  private final BooksQueryRepository repository;
}
