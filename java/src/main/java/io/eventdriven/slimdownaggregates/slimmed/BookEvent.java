package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;

public sealed interface BookEvent {
  record ChapterAdded(
    BookId bookId,
    Chapter chapter
  ) implements BookEvent {
  }

  record MovedToEditing(
    BookId bookId
  ) implements BookEvent {
  }

  record FormatAdded(
    BookId bookId,
    Format format
  ) implements BookEvent {
  }

  record FormatRemoved(
    BookId bookId,
    Format format
  ) implements BookEvent {
  }

  record TranslationAdded(
    BookId bookId,
    Translation translation
  ) implements BookEvent {
  }

  record Approved(
    BookId bookId,
    CommitteeApproval committeeApproval
  ) implements BookEvent {
  }

  record MovedToPrinting
    (
      BookId bookId
    ) implements BookEvent {
  }

  record Published(
    BookId bookId,
    ISBN isbn,
    Title title,
    Author author
  ) implements BookEvent {
  }

  record MovedToOutOfPrint
    (
      BookId bookId
    ) implements BookEvent {
  }
}
