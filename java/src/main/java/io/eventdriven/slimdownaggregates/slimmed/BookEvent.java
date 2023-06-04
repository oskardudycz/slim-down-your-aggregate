package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;

public sealed interface BookEvent {
  record ChapterAdded(
    BookId BookId,
    Chapter Chapter
  ) implements BookEvent {
  }

  record Approved(
    BookId BookId,
    CommitteeApproval CommitteeApproval
  ) implements BookEvent {
  }

  record MovedToEditing(
    BookId BookId
  ) implements BookEvent {
  }

  record MovedToPrinting
    (
      BookId BookId
    ) implements BookEvent {
  }

  record Published(
    BookId BookId,
    ISBN ISBN,
    Title Title,
    Author Author
  ) implements BookEvent {
  }

  record BookMovedToOutOfPrintEvent
    (
      BookId BookId
    ) implements BookEvent {
  }

  record FormatAdded(
    BookId BookId,
    Format Translation
  ) implements BookEvent {
  }

  record FormatRemoved(
    BookId BookId,
    Format Translation
  ) implements BookEvent {
  }

  record TranslationAdded(
    BookId BookId,
    Translation Translation
  ) implements BookEvent {
  }
}
