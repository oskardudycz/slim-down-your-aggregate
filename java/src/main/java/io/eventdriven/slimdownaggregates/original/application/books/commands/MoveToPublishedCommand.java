package io.eventdriven.slimdownaggregates.original.application.books.commands;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;

public record MoveToPublishedCommand(BookId bookId) {
}
