package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.UUID;

public class BookModel {

  private UUID id;
  private List<Chapter> chapters = new ArrayList<>();
  private CommitteeApproval committeeApproval;
  private List<Translation> translations = new ArrayList<>();
  private List<Format> formats = new ArrayList<>();
  private State currentState = State.WRITING;

  public enum State {WRITING, EDITING, PRINTING, PUBLISHED, OUT_OF_PRINT}

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

  public BookModel(BookId bookId, CommitteeApproval committeeApproval, Title title, Author author, Genre genre, List<Reviewer> reviewers,
                   Publisher publisher, ISBN isbn,
                   LocalDate publicationDate, int edition, int totalPages, int numberOfIllustrations,
                   String bindingType, String summary) {

    // Properties initialization
    this.id = bookId.getValue();
    this.bookId = bookId;
    this.committeeApproval = committeeApproval;
    this.title = title;
    this.author = author;
    this.genre = genre;
    this.reviewers = reviewers;
    this.publisher = publisher;
    this.isbn = isbn;
    this.publicationDate = publicationDate;
    this.edition = edition;
    this.totalPages = totalPages;
    this.numberOfIllustrations = numberOfIllustrations;
    this.bindingType = bindingType;
    this.summary = summary;
  }

  public UUID getId() {
    return id;
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

  public State getCurrentState() {
    return currentState;
  }

}

