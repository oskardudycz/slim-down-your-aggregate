package io.eventdriven.slimdownaggregates.original.entities;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;

@Entity
public class Office {
  @Id
  private Long id;

  public void setId(Long id) {
    this.id = id;
  }

  public Long getId() {
    return id;
  }
}
