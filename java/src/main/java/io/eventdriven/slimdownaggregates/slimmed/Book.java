package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;

import java.util.List;

import static io.eventdriven.slimdownaggregates.slimmed.BookEvent.*;
import static io.eventdriven.slimdownaggregates.slimmed.core.ListExtensions.except;
import static io.eventdriven.slimdownaggregates.slimmed.core.ListExtensions.union;

public record Book(
  BookId bookId,
  Title title,
  Author author,
  Genre genre,
  int reviewersCount,
  ISBN isbn,
  List<String> chapterTitles,
  int translationsCount,
  List<Format> formats,
  Book.State currentState,
  boolean isApproved
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
          state.reviewersCount,
          state.isbn,
          union(state.chapterTitles, chapterAdded.chapter().getTitle().getValue()),
          state.translationsCount,
          state.formats,
          state.currentState,
          state.isApproved
        );
      }
      case MovedToEditing ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount,
          state.formats,
          State.WRITING,
          state.isApproved
        );
      }
      case FormatAdded formatAdded: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount,
          union(state.formats, formatAdded.format()),
          state.currentState,
          state.isApproved
        );
      }
      case FormatRemoved formatRemoved: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount,
          except(state.formats, f -> f.getFormatType().equals(formatRemoved.format().getFormatType())),
          state.currentState,
          state.isApproved
        );
      }
      case TranslationAdded translationAdded: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount + 1,
          state.formats,
          state.currentState,
          state.isApproved
        );
      }
      case Approved approved: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount,
          state.formats,
          state.currentState,
          true
        );
      }
      case MovedToPrinting ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount,
          state.formats,
          State.EDITING,
          state.isApproved
        );
      }
      case Published ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount,
          state.formats,
          State.PRINTING,
          state.isApproved
        );
      }
      case MovedToOutOfPrint ignore: {
        yield new Book(
          state.bookId,
          state.title,
          state.author,
          state.genre,
          state.reviewersCount,
          state.isbn,
          state.chapterTitles,
          state.translationsCount,
          state.formats,
          State.OUT_OF_PRINT,
          state.isApproved
        );
      }
    };
  }
}

