package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.BookId;

public record BookMovedToPrintingEvent(
  BookId bookId
) implements IDomainEvent {
}
