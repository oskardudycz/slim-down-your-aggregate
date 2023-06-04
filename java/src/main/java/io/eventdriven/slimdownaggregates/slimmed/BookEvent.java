package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.core.IDomainEvent;
import io.eventdriven.slimdownaggregates.slimmed.entities.*;

public sealed interface BookEvent extends IDomainEvent {
  record ChapterAddedEvent(
    BookId BookId,
    Chapter Chapter
  ) implements BookEvent {
  }

  record BookApprovedEvent(
    BookId BookId,
    CommitteeApproval CommitteeApproval
  ) implements BookEvent {
  }

  record BookMovedToEditingEvent(
    BookId BookId
  ) implements BookEvent {
  }

  record BookMovedToPrintingEvent
    (
      BookId BookId
    ) implements BookEvent {
  }

  record BookPublishedEvent(
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

  record FormatAddedEvent(
    BookId BookId,
    Format Translation
  ) implements BookEvent {
  }

  record FormatRemovedEvent(
    BookId BookId,
    Format Translation
  ) implements BookEvent {
  }

  record TranslationAddedEvent(
    BookId BookId,
    Translation Translation
  ) implements BookEvent {
  }
}
