package io.eventdriven.slimdownaggregates.original.persistence.books.entities;

import jakarta.persistence.Embeddable;
import java.io.Serializable;
import java.util.Objects;
import java.util.UUID;

@Embeddable
public class ChapterId implements Serializable {

  private int number;
  private UUID bookId;

  public ChapterId() {}

  public ChapterId(int number, UUID bookId) {
    this.number = number;
    this.bookId = bookId;
  }

  // getters, setters

  @Override
  public boolean equals(Object o) {
    if (this == o) return true;
    if (o == null || getClass() != o.getClass()) return false;
    ChapterId chapterId = (ChapterId) o;
    return number == chapterId.number && Objects.equals(bookId, chapterId.bookId);
  }

  @Override
  public int hashCode() {
    return Objects.hash(number, bookId);
  }
}


