package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;

import java.util.UUID;

public record BookMovedToEditingEvent(UUID bookId) implements IDomainEvent {
}
