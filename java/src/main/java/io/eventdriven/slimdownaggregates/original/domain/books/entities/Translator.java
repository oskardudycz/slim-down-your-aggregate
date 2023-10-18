package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record Translator(TranslatorId id, TranslatorName name) {
  public Translator {
    assertNotNull(id);
    assertNotNull(name);
  }
}
