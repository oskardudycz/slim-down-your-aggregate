package io.eventdriven.slimdownaggregates.original.application.books.commands;

import io.eventdriven.slimdownaggregates.original.domain.books.authors.AuthorIdOrData;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.PositiveInt;

public record CreateDraftCommand(
  BookId bookId,
  Title title,
  AuthorIdOrData author,
  PublisherId publisherId,
  PositiveInt edition,
  Genre genre
) {
}
