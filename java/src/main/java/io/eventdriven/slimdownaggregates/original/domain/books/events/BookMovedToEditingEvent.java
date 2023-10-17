package io.eventdriven.slimdownaggregates.original.domain.books.events;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.infrastructure.events.DomainEvent;

public record BookMovedToEditingEvent(BookId bookId) implements DomainEvent {
}
