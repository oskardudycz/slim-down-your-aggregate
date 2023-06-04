package io.eventdriven.slimdownaggregates.original.events;

import io.eventdriven.slimdownaggregates.original.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.original.entities.BookId;

public record BookMovedToEditingEvent(BookId bookId) implements IDomainEvent {
}
