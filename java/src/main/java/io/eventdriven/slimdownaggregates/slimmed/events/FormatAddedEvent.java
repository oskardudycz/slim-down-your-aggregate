package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.BookId;
import io.eventdriven.slimdownaggregates.slimmed.entities.Format;

public record FormatAddedEvent(
  BookId bookId,
  Format format
) implements IDomainEvent {
}
