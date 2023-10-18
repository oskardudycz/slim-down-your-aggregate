package io.eventdriven.slimdownaggregates.shorter.original.events;

import io.eventdriven.slimdownaggregates.shorter.original.core.DomainEvent;
import io.eventdriven.slimdownaggregates.shorter.original.entities.BookId;

public record BookMovedToEditingEvent(BookId bookId) implements DomainEvent {
}
