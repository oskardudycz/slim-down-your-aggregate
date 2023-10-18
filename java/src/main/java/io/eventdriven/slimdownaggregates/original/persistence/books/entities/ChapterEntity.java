package io.eventdriven.slimdownaggregates.original.persistence.books.entities;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.Chapter;
import io.eventdriven.slimdownaggregates.original.persistence.books.BookEntity;
import jakarta.persistence.*;
import java.util.UUID;

@Entity
@Table(name = "chapters")
@IdClass(ChapterId.class)
public class ChapterEntity {

  @Id
  private int number;

  @Id
  @Column(name = "book_id")
  private UUID bookId;

  @Column(nullable = false)
  private String title;

  @Column(nullable = true)
  private String content = "";

  @ManyToOne
  @JoinColumn(name = "book_id", insertable = false, updatable = false)
  private BookEntity book;

  // Default constructor for JPA
  public ChapterEntity() {}

  // Getters and setters

  public int getNumber() {
    return number;
  }

  public void setNumber(int number) {
    this.number = number;
  }

  public UUID getBookId() {
    return bookId;
  }

  public void setBookId(UUID bookId) {
    this.bookId = bookId;
  }

  public String getTitle() {
    return title;
  }

  public void setTitle(String title) {
    this.title = title;
  }

  public String getContent() {
    return content;
  }

  public void setContent(String content) {
    this.content = content;
  }

  public BookEntity getBook() {
    return book;
  }

  public void setBook(BookEntity book) {
    this.book = book;
  }

  public ChapterEntity update(Chapter chapter) {
    if (!title.equals(chapter.title().value())) {
      title = chapter.title().value();
    }
    if (!content.equals(chapter.content().value())) {
      content = chapter.content().value();
    }
    return this;
  }
}


