package io.eventdriven.slimdownaggregates.original.persistence.books.entities;

import io.eventdriven.slimdownaggregates.original.persistence.books.BookEntity;
import jakarta.persistence.*;

import java.util.UUID;

@Entity
@Table(name = "formats")
@IdClass(FormatId.class)
public class FormatEntity {

  @Id
  @Column(name = "format_type", nullable = false)
  private String formatType;

  @Id
  @Column(name = "book_id", nullable = false)
  private UUID bookId;

  @Column(name = "total_copies", nullable = false)
  private int totalCopies;

  @Column(name = "sold_copies", nullable = false)
  private int soldCopies;

  @ManyToOne
  @JoinColumn(name = "book_id", insertable = false, updatable = false)
  private BookEntity book;

  // Default constructor for JPA
  public FormatEntity() {}

  // Default constructor for JPA
  public FormatEntity(UUID bookId, String formatType, int totalCopies, int soldCopies) {
    this.bookId = bookId;
    this.formatType = formatType;
    this.totalCopies = totalCopies;
    this.soldCopies = soldCopies;
  }

  // Getters and setters

  public String getFormatType() {
    return formatType;
  }

  public void setFormatType(String formatType) {
    this.formatType = formatType;
  }

  public UUID getBookId() {
    return bookId;
  }

  public void setBookId(UUID bookId) {
    this.bookId = bookId;
  }

  public int getTotalCopies() {
    return totalCopies;
  }

  public void setTotalCopies(int totalCopies) {
    this.totalCopies = totalCopies;
  }

  public int getSoldCopies() {
    return soldCopies;
  }

  public void setSoldCopies(int soldCopies) {
    this.soldCopies = soldCopies;
  }

  public BookEntity getBook() {
    return book;
  }

  public void setBook(BookEntity book) {
    this.book = book;
  }
}
