package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;

import static io.eventdriven.slimdownaggregates.slimmed.BookEvent.*;
import static io.eventdriven.slimdownaggregates.slimmed.Book.*;

public final class BookService {
  public static ChapterAdded addChapter(ChapterTitle title, ChapterContent content, Book state) {
    if (state.getChapters().stream().anyMatch(chap -> chap.getTitle().equals(title))) {
      throw new IllegalStateException("chapter with the same title already exists.");
    }

    if (!state.getChapters().isEmpty() && !state.getChapters().get(state.getChapters().size() - 1).getTitle().getValue().equals("chapter " + state.getChapters().size())) {
      throw new IllegalStateException(
        "chapter should be added in sequence. The title of the next chapter should be 'chapter " + (state.getChapters().size() + 1) + "'.");
    }

    var chapter = new Chapter(title, content);

    return new ChapterAdded(state.getBookId(), chapter);
  }

  public static MovedToEditing moveToEditing(Book state) {
    if (state.getCurrentState() != State.WRITING)
      throw new IllegalStateException("Cannot move to Editing state from the current state.");

    if (state.getChapters().size() < 1)
      throw new IllegalStateException("A book must have at least one chapter to move to the Editing state.");

    return new MovedToEditing(state.getBookId());
  }

  public static TranslationAdded addTranslation(Translation translation, Book state) {
    if (state.getCurrentState() != State.EDITING)
      throw new IllegalStateException("Cannot add translation of a book that is not in the Editing state.");

    if (state.getTranslations().size() >= 5)
      throw new IllegalStateException("Cannot add more state.getTranslations(). Maximum 5 state.getTranslations() are allowed.");

    return new TranslationAdded(state.getBookId(), translation);
  }

  public static FormatAdded addFormat(Format format, Book state) {
    if (state.getCurrentState() != State.EDITING)
      throw new IllegalStateException("Cannot add format of a book that is not in the Editing state.");

    if (state.getFormats().stream().anyMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("Format " + format.getFormatType() + " already exists.");

    return new FormatAdded(state.getBookId(), format);
  }

  public static FormatRemoved removeFormat(Format format, Book state) {
    if (state.getCurrentState() != State.EDITING)
      throw new IllegalStateException("Cannot remove format of a book that is not in the Editing state.");

    if (state.getFormats().stream().noneMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("Format " + format.getFormatType() + " does not exist.");

    return new FormatRemoved(state.getBookId(), format);
  }

  public static Approved approve(CommitteeApproval committeeApproval, Book state) {
    if (state.getCurrentState() != State.EDITING)
      throw new IllegalStateException("Cannot approve a book that is not in the Editing state.");

    if (state.getReviewers().size() < 3)
      throw new IllegalStateException(
        "A book cannot be approved unless it has been reviewed by at least three reviewers.");

    return new Approved(state.getBookId(), committeeApproval);
  }

  public static MovedToPrinting moveToPrinting(Book state) throws Exception {
    if (state.getCurrentState() != State.EDITING) {
      throw new Exception("Cannot move to Printing state from the current state.");
    }

    if (state.getCommitteeApproval() == null) {
      throw new Exception("Cannot move to the Printing state until the book has been approved.");
    }

    if (state.getReviewers().size() < 3) {
      throw new Exception(
        "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");
    }

    if (!state.getPublishingHouse().isGenreLimitReached(state.getGenre())) {
      throw new Exception("Cannot move to the Printing state until the genre limit is reached.");
    }

    return new MovedToPrinting(state.getBookId());
  }

  public static Published moveToPublished(Book state) {
    if (state.getCurrentState() != State.PRINTING || state.getTranslations().size() < 5)
      throw new IllegalStateException("Cannot move to Published state from the current state.");

    if (state.getReviewers().size() < 3)
      throw new IllegalStateException(
        "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

    return new Published(state.getBookId(), state.getIsbn(), state.getTitle(), state.getAuthor());
  }

  public static MovedToOutOfPrint moveToOutOfPrint(Book state) {
    if (state.getCurrentState() != State.PUBLISHED)
      throw new IllegalStateException("Cannot move to Out of Print state from the current state.");

    double totalCopies = state.getFormats().stream().mapToDouble(Format::getTotalCopies).sum();
    double totalSoldCopies = state.getFormats().stream().mapToDouble(Format::getSoldCopies).sum();
    if ((totalSoldCopies / totalCopies) > 0.1)
      throw new IllegalStateException(
        "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

    return new MovedToOutOfPrint(state.getBookId());
  }
}

