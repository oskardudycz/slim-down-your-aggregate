package io.eventdriven.slimdownaggregates.original.entities;

import io.eventdriven.slimdownaggregates.original.core.EnumOptionData;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;

@Entity
public class Staff {
  @Id
  private Long id;

  public void setId(Long id) {
    this.id = id;
  }

  public Long getId() {
    return id;
  }

  public EnumOptionData organisationalRoleData() {
    return null;
  }

  public String displayName() {
    return null;
  }
}
