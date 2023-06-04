package io.eventdriven.slimdownaggregates.original;

import io.eventdriven.slimdownaggregates.original.core.Aggregate;
import io.eventdriven.slimdownaggregates.original.entities.*;
import io.eventdriven.slimdownaggregates.original.events.BookMovedToEditingEvent;
import io.eventdriven.slimdownaggregates.original.events.BookPublishedEvent;
import io.eventdriven.slimdownaggregates.original.events.ChapterAddedEvent;
import io.eventdriven.slimdownaggregates.original.services.IPublishingHouse;

import java.time.LocalDate;
import java.util.*;

public class Book extends Aggregate {
  private List<Chapter> chapters = new ArrayList<>();
  private CommitteeApproval committeeApproval;
  private IPublishingHouse publishingHouse;
  private List<Translation> translations = new ArrayList<>();
  private List<Format> formats = new ArrayList<>();
  private State currentState = State.WRITING;

  public enum State { WRITING, EDITING, PRINTING, PUBLISHED, OUT_OF_PRINT }

  // Properties
  private BookId bookId;
  private Title title;
  private Author author;
  private Genre genre;
  private List<Reviewer> reviewers;
  private Publisher publisher;
  private ISBN isbn;
  private LocalDate publicationDate;
  private int edition;
  private int totalPages;
  private int numberOfIllustrations;
  private String bindingType;
  private String summary;

  public Book(BookId bookId, Title title, Author author, Genre genre, List<Reviewer> reviewers,
              IPublishingHouse publishingHouse, Publisher publisher, ISBN isbn,
              LocalDate publicationDate, int edition, int totalPages, int numberOfIllustrations,
              String bindingType, String summary) {
    super(bookId.getValue());

    this.bookId = bookId;
    this.title = title;
    this.author = author;
    this.genre = genre;
    this.reviewers = reviewers;
    this.publishingHouse = publishingHouse;
    this.publisher = publisher;
    this.isbn = isbn;
    this.publicationDate = publicationDate;
    this.edition = edition;
    this.totalPages = totalPages;
    this.numberOfIllustrations = numberOfIllustrations;
    this.bindingType = bindingType;
    this.summary = summary;
  }

  public void addChapter(ChapterTitle title, ChapterContent content) {
    if (chapters.stream().anyMatch(chap -> chap.getTitle().equals(title))) {
      throw new IllegalStateException("Chapter with the same title already exists.");
    }

    if (!chapters.isEmpty() && !chapters.get(chapters.size() - 1).getTitle().getValue().equals("Chapter " + chapters.size())) {
      throw new IllegalStateException(
        "Chapter should be added in sequence. The title of the next chapter should be 'Chapter " + (chapters.size() + 1) + "'.");
    }

    var chapter = new Chapter(title, content);
    chapters.add(chapter);

    addDomainEvent(new ChapterAddedEvent(this.bookId, chapter));
  }

  public void moveToEditing() {
    if (currentState != State.WRITING)
      throw new IllegalStateException("Cannot move to Editing state from the current state.");

    if (chapters.size() < 1)
      throw new IllegalStateException("A book must have at least one chapter to move to the Editing state.");

    currentState = State.EDITING;

    addDomainEvent(new BookMovedToEditingEvent(this.bookId));
  }

  public void addTranslation(Translation translation) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot add translation of a book that is not in the Editing state.");

    if (translations.size() >= 5)
      throw new IllegalStateException("Cannot add more translations. Maximum 5 translations are allowed.");

    translations.add(translation);
  }

  public void addFormat(Format format) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot add format of a book that is not in the Editing state.");

    if (formats.stream().anyMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("Format " + format.getFormatType() + " already exists.");

    formats.add(format);
  }

  public void removeFormat(Format format) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot remove format of a book that is not in the Editing state.");

    if (formats.stream().noneMatch(f -> f.getFormatType().equals(format.getFormatType())))
      throw new IllegalStateException("Format " + format.getFormatType() + " does not exist.");

    formats.removeIf(f -> f.getFormatType().equals(format.getFormatType()));
  }

  public void approve(CommitteeApproval committeeApproval) {
    if (currentState != State.EDITING)
      throw new IllegalStateException("Cannot approve a book that is not in the Editing state.");

    if (reviewers.size() < 3)
      throw new IllegalStateException(
        "A book cannot be approved unless it has been reviewed by at least three reviewers.");

    this.committeeApproval = committeeApproval;
  }

  public void moveToPrinting() throws Exception {
    if (this.currentState != State.EDITING) {
      throw new Exception("Cannot move to Printing state from the current state.");
    }

    if (this.committeeApproval == null) {
      throw new Exception("Cannot move to the Printing state until the book has been approved.");
    }

    if (this.reviewers.size() < 3) {
      throw new Exception(
        "A book cannot be moved to the Printing state unless it has been reviewed by at least three reviewers.");
    }

    if (!this.publishingHouse.isGenreLimitReached(this.genre)) {
      throw new Exception("Cannot move to the Printing state until the genre limit is reached.");
    }

    this.currentState = State.PRINTING;
  }

  public void moveToPublished() {
    if (currentState != State.PRINTING || translations.size() < 5)
      throw new IllegalStateException("Cannot move to Published state from the current state.");

    if (reviewers.size() < 3)
      throw new IllegalStateException(
        "A book cannot be moved to the Published state unless it has been reviewed by at least three reviewers.");

    currentState = State.PUBLISHED;

    addDomainEvent(new BookPublishedEvent(this.bookId, isbn, title, author));
  }

  public void moveToOutOfPrint() {
    if (currentState != State.PUBLISHED)
      throw new IllegalStateException("Cannot move to Out of Print state from the current state.");

    double totalCopies = formats.stream().mapToDouble(Format::getTotalCopies).sum();
    double totalSoldCopies = formats.stream().mapToDouble(Format::getSoldCopies).sum();
    if ((totalSoldCopies / totalCopies) > 0.1)
      throw new IllegalStateException(
        "Cannot move to Out of Print state if more than 10% of total copies are unsold.");

    currentState = State.OUT_OF_PRINT;
  }

  // Getter methods
  public BookId getBookId() {
    return bookId;
  }

  public Title getTitle() {
    return title;
  }

  public Author getAuthor() {
    return author;
  }

  public Genre getGenre() {
    return genre;
  }

  public List<Reviewer> getReviewers() {
    return Collections.unmodifiableList(reviewers);
  }

  public Publisher getPublisher() {
    return publisher;
  }

  public ISBN getIsbn() {
    return isbn;
  }

  public LocalDate getPublicationDate() {
    return publicationDate;
  }

  public int getEdition() {
    return edition;
  }

  public int getTotalPages() {
    return totalPages;
  }

  public int getNumberOfIllustrations() {
    return numberOfIllustrations;
  }

  public String getBindingType() {
    return bindingType;
  }

  public String getSummary() {
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


}

