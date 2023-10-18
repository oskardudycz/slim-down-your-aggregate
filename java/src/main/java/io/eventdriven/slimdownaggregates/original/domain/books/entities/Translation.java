package io.eventdriven.slimdownaggregates.original.domain.books.entities;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public record Translation (Language language, Translator translator){
  public Translation {
    assertNotNull(language);
    assertNotNull(translator);
  }
}
