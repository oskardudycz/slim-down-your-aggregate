package io.eventdriven.slimdownaggregates.slimmed;

import io.eventdriven.slimdownaggregates.slimmed.entities.*;
import io.eventdriven.slimdownaggregates.slimmed.services.IPublishingHouse;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.UUID;

import static io.eventdriven.slimdownaggregates.slimmed.BookEvent.*;

public class Book {
  private List<Chapter> chapters = new ArrayList<>();
  private CommitteeApproval committeeApproval;
  private IPublishingHouse publishingHouse;
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
  private ISBN isbn;

  public Book(BookId bookId, Title title, Author author, Genre genre, List<Reviewer> reviewers,
              IPublishingHouse publishingHouse, ISBN isbn) {
    this.bookId = bookId;
    this.title = title;
    this.author = author;
    this.genre = genre;
    this.reviewers = reviewers;
    this.publishingHouse = publishingHouse;
    this.isbn = isbn;
  }

  public <T extends BookEvent> T evolve(T event) {
    return switch (event) {
      case ChapterAdded chapterAdded: {
        chapters.add(chapterAdded.chapter());
        yield event;
      }
      case MovedToEditing ignore: {
        currentState = State.EDITING;
        yield event;
      }
      case FormatAdded formatAdded: {
        formats.add(formatAdded.format());
        yield event;
      }
      case FormatRemoved formatRemoved: {
        formats.removeIf(f -> f.getFormatType().equals(formatRemoved.format().getFormatType()));
        yield event;
      }
      case TranslationAdded translationAdded: {
        translations.add(translationAdded.translation());
        yield event;
      }
      case Approved approved: {
        this.committeeApproval = approved.committeeApproval();
        yield event;
      }
      case MovedToPrinting ignore: {
        currentState = State.EDITING;
        yield event;
      }
      case Published ignore: {
        this.currentState = State.PRINTING;
        yield event;
      }
      case MovedToOutOfPrint ignore: {
        this.currentState = State.OUT_OF_PRINT;
        yield event;
      }
    };
  }

  // Getter methods
  public UUID getId() {
    return bookId.getValue();
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

  public State getCurrentState() {
    return currentState;
  }

  public IPublishingHouse getPublishingHouse() {
    return publishingHouse;
  }

  public List<Reviewer> getReviewers() {
    return Collections.unmodifiableList(reviewers);
  }

  public ISBN getIsbn() {
    return isbn;
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

