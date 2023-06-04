package io.eventdriven.slimdownaggregates.original.events;


import io.eventdriven.slimdownaggregates.original.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.original.entities.Author;
import io.eventdriven.slimdownaggregates.original.entities.BookId;
import io.eventdriven.slimdownaggregates.original.entities.ISBN;
import io.eventdriven.slimdownaggregates.original.entities.Title;

public record BookPublishedEvent(
  BookId bookId,
  ISBN isbn,
  Title title,
  Author author
) implements IDomainEvent {
}
