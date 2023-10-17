package io.eventdriven.slimdownaggregates.original.application.books.commands;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;

public record SetISBNCommand(BookId bookId, ISBN isbn){
}
