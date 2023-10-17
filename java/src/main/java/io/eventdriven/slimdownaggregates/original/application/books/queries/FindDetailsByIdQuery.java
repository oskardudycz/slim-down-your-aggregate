package io.eventdriven.slimdownaggregates.original.application.books.queries;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.BookId;

public record FindDetailsByIdQuery(BookId bookId) {
}
