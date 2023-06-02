package io.eventdriven.slimdownaggregates.original.user.entities;

import jakarta.persistence.Entity;
import jakarta.persistence.Id;

@Entity
public class Permission {
  @Id
  private Long id;

  public void setId(Long id) {
    this.id = id;
  }

  public Long getId() {
    return id;
  }

  public String getCode() {
    return null;
  }
}
