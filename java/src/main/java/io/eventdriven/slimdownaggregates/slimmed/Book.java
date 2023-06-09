package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;

import java.util.ArrayList;
import java.util.List;

import static io.eventdriven.slimdownaggregates.slimmed.BookEvent.*;
import static io.eventdriven.slimdownaggregates.slimmed.core.ListExtensions.except;
import static io.eventdriven.slimdownaggregates.slimmed.core.ListExtensions.union;

public sealed interface Book {
  record Initial() implements Book {

  }

  record InWriting(
    BookId bookId,
    Genre genre,
    Title title,
    Author author,
    ISBN isbn,
    List<String> chapterTitles
  ) implements Book {
  }

  record InEditing(
    BookId bookId,
    Genre genre,
    Title title,
    Author author,
    ISBN isbn,
    List<Format> formats,
    int translationsCount,
    int reviewersCount,
    boolean isApproved
  ) implements Book {
  }


  record InPrinting(
    BookId bookId,
    Title title,
    Author author,
    ISBN isbn,
    List<Format> formats,
    int reviewersCount,

    int translationsCount
  ) implements Book {
  }

  record InPublishing(
    BookId bookId,
    List<Format> formats
  ) implements Book {
  }

  record OutOfPrint() implements Book {

  }

  static <T extends BookEvent> Book evolve(Book state, T event) {
    return switch (event) {
      case WritingStarted writingStarted: {
        if (!(state instanceof Book.Initial)) {
          yield state;
        }

        yield new Book.InWriting(
          writingStarted.bookId(),
          writingStarted.genre(),
          writingStarted.title(),
          writingStarted.author(),
          writingStarted.isbn(),
          new ArrayList<>()
        );
      }
      case ChapterAdded chapterAdded: {
        if (!(state instanceof Book.InWriting bookInWriting)) {
          yield state;
        }

        yield new Book.InWriting(
          bookInWriting.bookId(),
          bookInWriting.genre(),
          bookInWriting.title(),
          bookInWriting.author(),
          bookInWriting.isbn(),
          union(bookInWriting.chapterTitles(), chapterAdded.chapter().getTitle().getValue())
        );
      }
      case MovedToEditing ignore: {
        if (!(state instanceof Book.InWriting bookInWriting)) {
          yield state;
        }
        yield new Book.InEditing(
          bookInWriting.bookId(),
          bookInWriting.genre(),
          bookInWriting.title(),
          bookInWriting.author(),
          bookInWriting.isbn(),
          new ArrayList<>(),
          0,
          0,
          false
        );
      }
      case FormatAdded formatAdded: {
        if (!(state instanceof Book.InEditing bookInEditing)) {
          yield state;
        }
        yield new Book.InEditing(
          bookInEditing.bookId(),
          bookInEditing.genre(),
          bookInEditing.title(),
          bookInEditing.author(),
          bookInEditing.isbn(),
          union(bookInEditing.formats(), formatAdded.format()),
          bookInEditing.translationsCount(),
          bookInEditing.reviewersCount(),
          bookInEditing.isApproved()
        );
      }
      case FormatRemoved formatRemoved: {
        if (!(state instanceof Book.InEditing bookInEditing)) {
          yield state;
        }
        yield new Book.InEditing(
          bookInEditing.bookId(),
          bookInEditing.genre(),
          bookInEditing.title(),
          bookInEditing.author(),
          bookInEditing.isbn(),
          except(bookInEditing.formats(), f -> f.getFormatType().equals(formatRemoved.format().getFormatType())),
          bookInEditing.translationsCount(),
          bookInEditing.reviewersCount(),
          bookInEditing.isApproved()
        );
      }
      case TranslationAdded translationAdded: {
        if (!(state instanceof Book.InEditing bookInEditing)) {
          yield state;
        }
        yield new Book.InEditing(
          bookInEditing.bookId(),
          bookInEditing.genre(),
          bookInEditing.title(),
          bookInEditing.author(),
          bookInEditing.isbn(),
          bookInEditing.formats(),
          bookInEditing.translationsCount() + 1,
          bookInEditing.reviewersCount(),
          bookInEditing.isApproved()
        );
      }
      case Approved approved: {
        if (!(state instanceof Book.InEditing bookInEditing)) {
          yield state;
        }
        yield new Book.InEditing(
          bookInEditing.bookId(),
          bookInEditing.genre(),
          bookInEditing.title(),
          bookInEditing.author(),
          bookInEditing.isbn(),
          bookInEditing.formats(),
          bookInEditing.translationsCount() + 1,
          bookInEditing.reviewersCount(),
          true
        );
      }
      case MovedToPrinting ignore: {
        if (!(state instanceof Book.InEditing bookInEditing)) {
          yield state;
        }

        yield new Book.InPrinting(
          bookInEditing.bookId(),
          bookInEditing.title(),
          bookInEditing.author(),
          bookInEditing.isbn(),
          bookInEditing.formats(),
          bookInEditing.reviewersCount,
          bookInEditing.translationsCount
        );
      }
      case Published ignore: {
        if (!(state instanceof Book.InPrinting bookInPrinting)) {
          yield state;
        }

        yield new InPublishing(
          bookInPrinting.bookId,
          bookInPrinting.formats()
        );
      }
      case MovedToOutOfPrint ignore: {
        if (!(state instanceof InPublishing)) {
          yield state;
        }
        yield new Book.OutOfPrint();
      }
    };
  }
}

