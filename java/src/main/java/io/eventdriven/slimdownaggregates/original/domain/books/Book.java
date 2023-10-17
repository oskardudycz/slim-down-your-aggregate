package io.eventdriven.slimdownaggregates.original.domain.books;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.*;
import io.eventdriven.slimdownaggregates.original.domain.books.events.BookMovedToEditingEvent;
import io.eventdriven.slimdownaggregates.original.domain.books.events.BookPublishedEvent;
import io.eventdriven.slimdownaggregates.original.domain.books.events.ChapterAddedEvent;
import io.eventdriven.slimdownaggregates.original.domain.books.factories.BookFactory;
import io.eventdriven.slimdownaggregates.original.domain.books.services.PublishingHouse;
import io.eventdriven.slimdownaggregates.original.infrastructure.aggregates.Aggregate;
import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.NonEmptyString;
import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.PositiveInt;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public class Book extends Aggregate<BookId> {
  private State currentState = State.WRITING;
  private final Title title;
  private final Author author;
  private final Publisher publisher;
  private final PositiveInt edition;
  // Properties
  private final Genre genre;
  private ISBN isbn;
  private final LocalDate publicationDate;
  private final PositiveInt totalPages;
  private final PositiveInt numberOfIllustrations;
  private final NonEmptyString bindingType;
  private final NonEmptyString summary;
  private CommitteeApproval committeeApproval;
  private final PublishingHouse publishingHouse;
  private final List<Chapter> chapters;
  private final List<Reviewer> reviewers;
  private final List<Translation> translations;
  private final List<Format> formats;

  private Book(
    BookId bookId,
    State state,
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
    List<Format> formats) {
    super(bookId);

    assertNotNull(title);
    assertNotNull(author);
    assertNotNull(publishingHouse);
    assertNotNull(publisher);
    assertNotNull(edition);

    this.currentState = state;
    this.title = title;
    this.author = author;
    this.publishingHouse = publishingHouse;
    this.publisher = publisher;
    this.edition = edition;
    this.genre = genre;
    this.isbn = isbn;
    this.publicationDate = publicationDate;
    this.totalPages = totalPages;
    this.numberOfIllustrations = numberOfIllustrations;
    this.bindingType = bindingType;
    this.summary = summary;
    this.committeeApproval = committeeApproval;
    this.reviewers = reviewers;
    this.chapters = chapters != null ? chapters : new ArrayList<>();
    this.translations = translations != null ? translations : new ArrayList<>();
    this.formats = formats != null ? formats : new ArrayList<>();
  }

  public static Book createDraft(
    BookId bookId,
    Title title,
    Author author,
    PublishingHouse publishingHouse,
    Publisher publisher,
    PositiveInt edition,
    Genre genre
  ) {
    return new Book(
      bookId, State.WRITING, title, author, publishingHouse, publisher, edition, genre,
      null, null, null, null, null, null,
      null, null, null, null, null
    );
  }

  public void addChapter(ChapterTitle title, ChapterContent content) {
    if (chapters.stream().anyMatch(chap -> chap.title().equals(title))) {
      throw new IllegalStateException("chapter with the same title already exists.");
    }

    if (!chapters.isEmpty() && !chapters.get(chapters.size() - 1).title().value().equals("chapter " + chapters.size())) {
      throw new IllegalStateException(
        "chapter should be added in sequence. The title of the next chapter should be 'chapter " + (chapters.size() + 1) + "'.");
    }

    var chapter = new Chapter(new ChapterNumber(chapters.size() + 1), title, content);
    chapters.add(chapter);

    addDomainEvent(new ChapterAddedEvent(this.id, chapter));
  }

  public void moveToEditing() {
    if (currentState != State.WRITING)
      throw new IllegalStateException("Cannot move to Editing state from the current state.");

    if (chapters.isEmpty())
      throw new IllegalStateException("A book must have at least one chapter to move to the Editing state.");

    if (genre == null)
      throw new IllegalStateException("A book can be moved to the editing only when genre is specified");

    currentState = State.EDITING;

    addDomainEvent(new BookMovedToEditingEvent(this.id));
  }

  public void addTranslation(Translation translation) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot add translation of a book that is not in the Editing state.");

    if (translations.size() >= 5)
      throw new IllegalStateException("Cannot add more translationsCount. Maximum 5 translationsCount are allowed.");

    translations.add(translation);
  }

  public void addFormat(Format format) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot add format of a book that is not in the Editing state.");

    if (formats.stream().anyMatch(f -> f.formatType().equals(format.formatType())))
      throw new IllegalStateException("format " + format.formatType() + " already exists.");

    formats.add(format);
  }

  public void removeFormat(Format format) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot remove format of a book that is not in the Editing state.");

    if (formats.stream().noneMatch(f -> f.formatType().equals(format.formatType())))
      throw new IllegalStateException("format " + format.formatType() + " does not exist.");

    formats.removeIf(f -> f.formatType().equals(format.formatType()));
  }

  public void addReviewer(Reviewer reviewer) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot add format of a book that is not in the Editing state.");

    if (reviewers.stream().anyMatch(f -> f.id().equals(reviewer.id())))
      throw new IllegalStateException(reviewer.name() + "  is already a reviewer..");

    reviewers.add(reviewer);
  }

  public void approve(CommitteeApproval committeeApproval) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot approve a book that is not in the Editing state.");

    if (reviewers.size() < 3)
      throw new IllegalStateException(
        "A book cannot be approved unless it has been reviewed by at least three reviewersCount.");

    this.committeeApproval = committeeApproval;
  }

  public void setISBN(ISBN isbn) {
    if (this.currentState != State.EDITING)
      throw new IllegalStateException("Cannot approve a book that is not in the Editing state.");

    if (this.isbn != null)
      throw new IllegalStateException("Cannot change already set ISBN.");

    this.isbn = isbn;
  }

  public void moveToPrinting() {
    if (this.currentState != State.EDITING) {
      throw new IllegalStateException("Cannot move to Printing state from the current state.");
    }

    if (this.chapters.isEmpty()) {
      throw new IllegalStateException("A book must have at least one chapter to move to the printing state.");
    }

    if (this.committeeApproval == null) {
      throw new IllegalStateException("Cannot move to the Printing state until the book has been approved.");
    }

    if (this.reviewers.size() < 3) {
      throw new IllegalStateException(
        "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewersCount.");
    }

    if (genre == null) {
      throw new IllegalStateException("Book can be moved to the printing only when genre is specified");
    }

    if (this.publishingHouse.isGenreLimitReached(this.genre)) {
      throw new IllegalStateException("Cannot move to the Printing state until the genre limit is reached.");
    }

    this.currentState = State.PRINTING;
  }

  public void moveToPublished() {
    if (currentState != State.PRINTING || translations.size() < 5)
      throw new IllegalStateException("Cannot move to Published state from the current state.");

    if (isbn == null) {
      throw new IllegalStateException("Cannot move to Published state without ISBN.");
    }

    if (reviewers.size() < 3)
      throw new IllegalStateException(
        "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewersCount.");

    currentState = State.PUBLISHED;

    addDomainEvent(new BookPublishedEvent(this.id, isbn, title, author));
  }

  public void moveToOutOfPrint() {
    if (currentState != State.PUBLISHED)
      throw new IllegalStateException("Cannot move to Out of Print state from the current state.");

    double totalCopies = formats.stream().mapToDouble(d -> d.totalCopies().value()).sum();
    double totalSoldCopies = formats.stream().mapToDouble(d -> d.soldCopies().value()).sum();
    if ((totalSoldCopies / totalCopies) > 0.1)
      throw new IllegalStateException(
        "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

    currentState = State.OUT_OF_PRINT;
  }

  public State currentState() {
    return currentState;
  }

  public Title title() {
    return title;
  }

  public Author author() {
    return author;
  }

  public Genre genre() {
    return genre;
  }

  public Publisher publisher() {
    return publisher;
  }

  public PositiveInt getEdition() {
    return edition;
  }

  public PublishingHouse publishingHouse() {
    return publishingHouse;
  }

  public List<Reviewer> reviewers() {
    return Collections.unmodifiableList(reviewers);
  }

  public ISBN isbn() {
    return isbn;
  }

  public LocalDate publicationDate() {
    return publicationDate;
  }

  public PositiveInt getTotalPages() {
    return totalPages;
  }

  public PositiveInt getNumberOfIllustrations() {
    return numberOfIllustrations;
  }

  public NonEmptyString getBindingType() {
    return bindingType;
  }

  public NonEmptyString getSummary() {
    return summary;
  }

  public List<Chapter> getChapters() {
    return Collections.unmodifiableList(chapters);
  }

  public CommitteeApproval getCommitteeApproval() {
    return committeeApproval;
  }

  public List<Translation> getTranslations() {
    return Collections.unmodifiableList(translations);
  }

  public List<Format> getFormats() {
    return Collections.unmodifiableList(formats);
  }

  public enum State {WRITING, EDITING, PRINTING, PUBLISHED, OUT_OF_PRINT}

  public class Factory implements BookFactory {

    @Override
    public Book Create(
      BookId bookId,
      State state,
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
      List<Format> formats) {
      return new Book(
        bookId, state, title, author, publishingHouse,
        publisher, edition, genre, isbn, publicationDate, totalPages,
        numberOfIllustrations, bindingType, summary, committeeApproval,
        reviewers, chapters, translations, formats);
    }
  }
}
