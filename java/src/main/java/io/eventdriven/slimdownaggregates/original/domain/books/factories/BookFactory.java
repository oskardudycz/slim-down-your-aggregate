package io.eventdriven.slimdownaggregates.original.domain.books.factories;

import io.eventdriven.slimdownaggregates.original.domain.books.Book;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.domain.books.services.PublishingHouse;
import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.*;

import java.time.LocalDate;
import java.util.List;

public interface BookFactory {
  Book Create(
    BookId bookId,
    Book.State state,
    Title title,
    Author author,
    PublishingHouse publishingHouse,
    Publisher publisher,
    PositiveInt edition,
    Genre genre,
    ISBN isbn,
    LocalDate publicationDate,
    PositiveInt totalPages,
    PositiveInt numberOfIllustrations,
    NonEmptyString bindingType,
    NonEmptyString summary,
    CommitteeApproval committeeApproval,
    List<Reviewer> reviewers,
    List<Chapter> chapters,
    List<Translation> translations,
    List<Format> formats
  );
}
