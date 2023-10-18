package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record Language(LanguageId id, LanguageName name) {
  public Language {
    assertNotNull(id);
    assertNotNull(name);
  }
}
