package io.eventdriven.slimdownaggregates.original.domain.books.events;


import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.infrastructure.events.DomainEvent;

public record ChapterAddedEvent(BookId bookId, Chapter chapter) implements DomainEvent {
}
