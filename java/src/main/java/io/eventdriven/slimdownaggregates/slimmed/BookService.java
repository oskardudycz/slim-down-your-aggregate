package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;
import io.eventdriven.slimdownaggregates.slimmed.services.PublishingHouse;

import static io.eventdriven.slimdownaggregates.slimmed.core.TypeExtensions.*;
import static io.eventdriven.slimdownaggregates.slimmed.BookEvent.*;
import static io.eventdriven.slimdownaggregates.slimmed.BookService.BookCommand.*;

public final class BookService {
  public sealed interface BookCommand {
    record StartWriting(
      BookId bookId,
      Genre genre,
      Title title,
      Author author,
      ISBN isbn
    ) implements BookCommand {
    }

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

    record Print(
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


  public static BookEvent decide(PublishingHouse publishingHouse, BookCommand command, Book state)
  {
    return switch (command)
    {
      case StartWriting startWriting -> handle(startWriting, ofType(state, Book.Initial.class));
      case AddChapter addChapter -> handle(addChapter, ofType(state, Book.InWriting.class));
      case Edit edit -> handle(edit, ofType(state, Book.InWriting.class));
      case AddFormat addFormat -> handle(addFormat, ofType(state, Book.InEditing.class));
      case RemoveFormat removeFormat -> handle(removeFormat, ofType(state, Book.InEditing.class));
      case AddTranslation addTranslation -> handle(addTranslation, ofType(state, Book.InEditing.class));
      case Approve approve -> handle(approve, ofType(state, Book.InEditing.class));
      case Print print -> handle(publishingHouse, print, ofType(state, Book.InEditing.class));
      case Publish publish -> handle(publish, ofType(state, Book.InPrinting.class));
      case MoveToOutOfPrint moveToOutOfPrint -> handle(moveToOutOfPrint, ofType(state, Book.InPublishing.class));
    };
  }

  public static WritingStarted handle(StartWriting command, Book.Initial state)
  {
    return new WritingStarted(
      command.bookId,
      command.genre,
      command.title,
      command.author,
      command.isbn
    );
  }

  public static ChapterAdded handle(AddChapter command, Book.InWriting state) {
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

  public static MovedToEditing handle(Edit command, Book.InWriting state) {
    if (state.chapterTitles().size() < 1)
      throw new IllegalStateException("A book must have at least one chapter to move to the Editing state.");

    return new MovedToEditing(state.bookId());
  }

  public static TranslationAdded handle(AddTranslation command, Book.InEditing state) {
    if (state.translationsCount() >= 5)
      throw new IllegalStateException("Cannot add more state.translationsCount(). Maximum 5 state.translationsCount() are allowed.");

    return new TranslationAdded(state.bookId(), command.translation());
  }

  public static FormatAdded handle(AddFormat command, Book.InEditing state) {
    var format = command.format();

    if (state.formats().stream().anyMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("format " + format.getFormatType() + " already exists.");

    return new FormatAdded(state.bookId(), format);
  }

  public static FormatRemoved handle(RemoveFormat command, Book.InEditing state) {
    var format = command.format();
    if (state.formats().stream().noneMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("format " + format.getFormatType() + " does not exist.");

    return new FormatRemoved(state.bookId(), format);
  }

  public static Approved handle(Approve command, Book.InEditing state) {
    if (state.reviewersCount() < 3)
      throw new IllegalStateException(
        "A book cannot be approved unless it has been reviewed by at least three reviewersCount.");

    return new Approved(state.bookId(), command.committeeApproval());
  }

  public static MovedToPrinting handle(PublishingHouse publishingHouse, Print command, Book.InEditing state) {
    if (!state.isApproved()) {
      throw new IllegalStateException("Cannot move to the Printing state until the book has been approved.");
    }

    if (state.reviewersCount() < 3) {
      throw new IllegalStateException(
        "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewersCount.");
    }

    if (!publishingHouse.isGenreLimitReached(state.genre())) {
      throw new IllegalStateException("Cannot move to the Printing state until the genre limit is reached.");
    }

    return new MovedToPrinting(state.bookId());
  }

  public static Published handle(Publish command, Book.InPrinting state) {
    if (state.translationsCount() < 5)
      throw new IllegalStateException("Cannot move to Published state from the current state.");

    if (state.reviewersCount() < 3)
      throw new IllegalStateException(
        "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewersCount.");

    return new Published(state.bookId(), state.isbn(), state.title(), state.author());
  }

  public static MovedToOutOfPrint handle(MoveToOutOfPrint command, Book.InPublishing state) {
    double totalCopies = state.formats().stream().mapToDouble(Format::getTotalCopies).sum();
    double totalSoldCopies = state.formats().stream().mapToDouble(Format::getSoldCopies).sum();
    if ((totalSoldCopies / totalCopies) > 0.1)
      throw new IllegalStateException(
        "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

    return new MovedToOutOfPrint(state.bookId());
  }
}

