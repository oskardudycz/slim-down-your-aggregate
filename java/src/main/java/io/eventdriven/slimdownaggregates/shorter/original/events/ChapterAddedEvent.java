package io.eventdriven.slimdownaggregates.shorter.original.events;

import io.eventdriven.slimdownaggregates.shorter.original.core.DomainEvent;
import io.eventdriven.slimdownaggregates.shorter.original.entities.BookId;
import io.eventdriven.slimdownaggregates.shorter.original.entities.Chapter;

public record ChapterAddedEvent(BookId bookId, Chapter chapter) implements DomainEvent {
}
