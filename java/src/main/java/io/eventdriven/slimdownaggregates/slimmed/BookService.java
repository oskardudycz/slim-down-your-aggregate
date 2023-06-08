package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;
import io.eventdriven.slimdownaggregates.slimmed.services.IPublishingHouse;

import static io.eventdriven.slimdownaggregates.slimmed.Book.State;
import static io.eventdriven.slimdownaggregates.slimmed.BookEvent.*;
import static io.eventdriven.slimdownaggregates.slimmed.BookService.BookCommand.*;

public final class BookService {
  public sealed interface BookCommand {
    record AddChapter(
      BookId bookId,
      ChapterTitle title,
      ChapterContent content
    ) implements BookCommand {
    }

    record AddFormat(
      BookId bookId,
      Format format
    ) implements BookCommand {
    }

    record RemoveFormat(
      BookId bookId,
      Format format
    ) implements BookCommand {
    }

    record AddTranslation(
      BookId bookId,
      Translation translation
    ) implements BookCommand {
    }

    record Edit(
      BookId bookId
    ) implements BookCommand {
    }

    record Approve(
      BookId bookId,
      CommitteeApproval committeeApproval
    ) implements BookCommand {
    }

    record Print
      (
        BookId bookId
      ) implements BookCommand {
    }

    record Publish(
      BookId bookId,
      ISBN isbn,
      Title title,
      Author author
    ) implements BookCommand {
    }

    record MoveToOutOfPrint
      (
        BookId bookId
      ) implements BookCommand {
    }
  }

  public static ChapterAdded addChapter(AddChapter command, Book state) {
    var title = command.title();
    var content = command.content();

    if (!state.chapterTitles().contains(title.getValue())) {
      throw new IllegalStateException("chapter with the same title already exists.");
    }

    if (!title.getValue().equals("chapter %s".formatted(state.chapterTitles().size()))) {
      throw new IllegalStateException(
        "chapter should be added in sequence. The title of the next chapter should be 'chapter " + (state.chapterTitles().size()) + "'.");
    }

    var chapter = new Chapter(title, content);

    return new ChapterAdded(state.bookId(), chapter);
  }

  public static MovedToEditing moveToEditing(Edit command, Book state) {
    if (state.currentState() != State.WRITING)
      throw new IllegalStateException("Cannot move to Editing state from the current state.");

    if (state.chapterTitles().size() < 1)
      throw new IllegalStateException("A book must have at least one chapter to move to the Editing state.");

    return new MovedToEditing(state.bookId());
  }

  public static TranslationAdded addTranslation(AddTranslation command, Book state) {
    if (state.currentState() != State.EDITING)
      throw new IllegalStateException("Cannot add translation of a book that is not in the Editing state.");

    if (state.translationsCount() >= 5)
      throw new IllegalStateException("Cannot add more state.translationsCount(). Maximum 5 state.translationsCount() are allowed.");

    return new TranslationAdded(state.bookId(), command.translation());
  }

  public static FormatAdded addFormat(AddFormat command, Book state) {
    var format = command.format();

    if (state.currentState() != State.EDITING)
      throw new IllegalStateException("Cannot add format of a book that is not in the Editing state.");

    if (state.formats().stream().anyMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("format " + format.getFormatType() + " already exists.");

    return new FormatAdded(state.bookId(), format);
  }

  public static FormatRemoved removeFormat(RemoveFormat command, Book state) {
    var format = command.format();
    if (state.currentState() != State.EDITING)
      throw new IllegalStateException("Cannot remove format of a book that is not in the Editing state.");

    if (state.formats().stream().noneMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("format " + format.getFormatType() + " does not exist.");

    return new FormatRemoved(state.bookId(), format);
  }

  public static Approved approve(Approve command, Book state) {
    if (state.currentState() != State.EDITING)
      throw new IllegalStateException("Cannot approve a book that is not in the Editing state.");

    if (state.reviewersCount() < 3)
      throw new IllegalStateException(
        "A book cannot be approved unless it has been reviewed by at least three reviewersCount.");

    return new Approved(state.bookId(), command.committeeApproval());
  }

  public static MovedToPrinting moveToPrinting(IPublishingHouse publishingHouse, Print command, Book state) throws Exception {
    if (state.currentState() != State.EDITING) {
      throw new Exception("Cannot move to Printing state from the current state.");
    }

    if (!state.isApproved()) {
      throw new Exception("Cannot move to the Printing state until the book has been approved.");
    }

    if (state.reviewersCount() < 3) {
      throw new Exception(
        "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewersCount.");
    }

    if (!publishingHouse.isGenreLimitReached(state.genre())) {
      throw new Exception("Cannot move to the Printing state until the genre limit is reached.");
    }

    return new MovedToPrinting(state.bookId());
  }

  public static Published moveToPublished(Publish command, Book state) {
    if (state.currentState() != State.PRINTING || state.translationsCount() < 5)
      throw new IllegalStateException("Cannot move to Published state from the current state.");

    if (state.reviewersCount() < 3)
      throw new IllegalStateException(
        "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewersCount.");

    return new Published(state.bookId(), state.isbn(), state.title(), state.author());
  }

  public static MovedToOutOfPrint moveToOutOfPrint(MoveToOutOfPrint command, Book state) {
    if (state.currentState() != State.PUBLISHED)
      throw new IllegalStateException("Cannot move to Out of Print state from the current state.");

    double totalCopies = state.formats().stream().mapToDouble(Format::getTotalCopies).sum();
    double totalSoldCopies = state.formats().stream().mapToDouble(Format::getSoldCopies).sum();
    if ((totalSoldCopies / totalCopies) > 0.1)
      throw new IllegalStateException(
        "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

    return new MovedToOutOfPrint(state.bookId());
  }
}

