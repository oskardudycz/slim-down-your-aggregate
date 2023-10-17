package io.eventdriven.slimdownaggregates.shorter.original.events;


import io.eventdriven.slimdownaggregates.shorter.original.core.DomainEvent;
import io.eventdriven.slimdownaggregates.shorter.original.entities.Author;
import io.eventdriven.slimdownaggregates.shorter.original.entities.BookId;
import io.eventdriven.slimdownaggregates.shorter.original.entities.ISBN;
import io.eventdriven.slimdownaggregates.shorter.original.entities.Title;

public record BookPublishedEvent(
  BookId bookId,
  ISBN isbn,
  Title title,
  Author author
) implements DomainEvent {
}
