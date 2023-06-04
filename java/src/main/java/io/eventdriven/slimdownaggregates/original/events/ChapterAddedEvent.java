package io.eventdriven.slimdownaggregates.original.events;

import io.eventdriven.slimdownaggregates.original.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.original.entities.BookId;
import io.eventdriven.slimdownaggregates.original.entities.Chapter;

public record ChapterAddedEvent(BookId bookId, Chapter chapter) implements IDomainEvent {
}
