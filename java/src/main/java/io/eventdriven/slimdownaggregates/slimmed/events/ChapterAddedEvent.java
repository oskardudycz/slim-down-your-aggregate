package io.eventdriven.slimdownaggregates.slimmed.events;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.BookId;
import io.eventdriven.slimdownaggregates.slimmed.entities.Chapter;

import java.util.UUID;

public record ChapterAddedEvent(BookId bookId, Chapter chapter) implements IDomainEvent {
}
