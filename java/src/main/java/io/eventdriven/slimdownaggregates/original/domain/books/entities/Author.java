package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record Author(
  AuthorId id,
  AuthorFirstName firstName,
  AuthorLastName lastName
) {
  public Author{
    assertNotNull(id);
    assertNotNull(firstName);
    assertNotNull(lastName);
  }
}
