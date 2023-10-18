package io.eventdriven.slimdownaggregates.original.persistence.books.entities;

import java.io.Serializable;
import java.util.Objects;
import java.util.UUID;

public class FormatId implements Serializable {

  private String formatType;
  private UUID bookId;

  // Default constructor, getters, setters, equals, and hashCode

  public FormatId() {}

  public FormatId(String formatType, UUID bookId) {
    this.formatType = formatType;
    this.bookId = bookId;
  }

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

  @Override
  public boolean equals(Object o) {
    if (this == o) return true;
    if (o == null || getClass() != o.getClass()) return false;
    FormatId formatId = (FormatId) o;
    return formatType.equals(formatId.formatType) &&
      bookId.equals(formatId.bookId);
  }

  @Override
  public int hashCode() {
    return Objects.hash(formatType, bookId);
  }
}

