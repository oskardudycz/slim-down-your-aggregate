package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.BookId;
import io.eventdriven.slimdownaggregates.slimmed.entities.Translation;

public record TranslationAddedEvent(
  BookId bookId,
  Translation translation
) implements IDomainEvent {
}
