package io.eventdriven.slimdownaggregates.original.domain.books.authors;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.Author;

public interface AuthorProvider {
  Author getOrCreate(AuthorIdOrData authorIdOrData);
}
