package io.eventdriven.slimdownaggregates.original.domain.books.authors;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.AuthorFirstName;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.AuthorId;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.AuthorLastName;

public record AuthorIdOrData(
  AuthorId authorId,
  AuthorFirstName firstName,
  AuthorLastName lastName) {
  public AuthorIdOrData {
    if ((authorId != null && (firstName == null || lastName == null)) ||
      (authorId == null && (firstName == null || lastName == null))) {
      throw new IllegalArgumentException("either authorId or first name and last name has to be provided");
    }
  }
}
