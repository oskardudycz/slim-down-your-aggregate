package io.eventdriven.slimdownaggregates.original.domain.books.events;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.infrastructure.events.DomainEvent;

public record BookPublishedEvent(
  BookId bookId,
  ISBN isbn,
  Title title,
  Author author
) implements DomainEvent {
}
