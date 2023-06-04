package io.eventdriven.slimdownaggregates.original.events;


import io.eventdriven.slimdownaggregates.original.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.original.entities.Author;
import io.eventdriven.slimdownaggregates.original.entities.ISBN;
import io.eventdriven.slimdownaggregates.original.entities.Title;

import java.util.UUID;

public record BookPublishedEvent(
  UUID bookId,
  ISBN isbn,
  Title title,
  Author author
) implements IDomainEvent {
}
