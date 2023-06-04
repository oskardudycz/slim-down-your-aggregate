package io.eventdriven.slimdownaggregates.original.events;

import io.eventdriven.slimdownaggregates.original.core.IDomainEvent;

import java.util.UUID;

public record BookMovedToEditingEvent(UUID bookId) implements IDomainEvent {
}
