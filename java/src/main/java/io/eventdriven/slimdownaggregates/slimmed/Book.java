package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;
import io.eventdriven.slimdownaggregates.slimmed.services.IPublishingHouse;

import java.util.List;

import static io.eventdriven.slimdownaggregates.slimmed.BookEvent.*;
import static io.eventdriven.slimdownaggregates.slimmed.core.ListExtensions.*;

public record Book(
  BookId bookId,
  Title title,
  Author author,
  Genre genre,
  List<Reviewer> reviewers,
  IPublishingHouse publishingHouse,
  ISBN isbn,
  List<Chapter> chapters,
  List<Translation> translations,
  List<Format> formats,
  Book.State currentState,
  CommitteeApproval committeeApproval
) {
  public enum State {WRITING, EDITING, PRINTING, PUBLISHED, OUT_OF_PRINT}

  public static <T extends BookEvent> Book evolve(Book state, T event) {
    return switch (event) {
      case ChapterAdded chapterAdded: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          union(state.chapters, chapterAdded.chapter()),
          state.translations,
          state.formats,
          state.currentState,
          state.committeeApproval
        );
      }
      case MovedToEditing ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          state.translations,
          state.formats,
          State.WRITING,
          state.committeeApproval
        );
      }
      case FormatAdded formatAdded: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          state.translations,
          union(state.formats, formatAdded.format()),
          state.currentState,
          state.committeeApproval
        );
      }
      case FormatRemoved formatRemoved: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          state.translations,
          except(state.formats, f -> f.getFormatType().equals(formatRemoved.format().getFormatType())),
          state.currentState,
          state.committeeApproval
        );
      }
      case TranslationAdded translationAdded: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          union(state.translations, translationAdded.translation()),
          state.formats,
          state.currentState,
          state.committeeApproval
        );
      }
      case Approved approved: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          state.translations,
          state.formats,
          state.currentState,
          approved.committeeApproval()
        );
      }
      case MovedToPrinting ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          state.translations,
          state.formats,
          State.EDITING,
          state.committeeApproval
        );
      }
      case Published ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          state.translations,
          state.formats,
          State.PRINTING,
          state.committeeApproval
        );
      }
      case MovedToOutOfPrint ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewers,
          state.publishingHouse,
          state.isbn,
          state.chapters,
          state.translations,
          state.formats,
          State.OUT_OF_PRINT,
          state.committeeApproval
        );
      }
    };
  }
}

