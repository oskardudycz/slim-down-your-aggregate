package io.eventdriven.slimdownaggregates.slimmed.events;


import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.Author;
import io.eventdriven.slimdownaggregates.slimmed.entities.ISBN;
import io.eventdriven.slimdownaggregates.slimmed.entities.Title;

import java.util.UUID;

public record BookPublishedEvent(
  UUID bookId,
  ISBN isbn,
  Title title,
  Author author
) implements IDomainEvent {
}
