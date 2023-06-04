package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.BookId;

import java.util.UUID;

public record BookMovedToEditingEvent(BookId bookId) implements IDomainEvent {
}
