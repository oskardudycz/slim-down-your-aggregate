package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.Chapter;

import java.util.UUID;

public record ChapterAddedEvent(UUID bookId, Chapter chapter) implements IDomainEvent {
}
