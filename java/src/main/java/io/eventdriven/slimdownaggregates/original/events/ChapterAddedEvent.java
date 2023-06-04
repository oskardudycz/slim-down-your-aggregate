package io.eventdriven.slimdownaggregates.original.events;

import io.eventdriven.slimdownaggregates.original.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.original.entities.Chapter;

import java.util.UUID;

public record ChapterAddedEvent(UUID bookId, Chapter chapter) implements IDomainEvent {
}
