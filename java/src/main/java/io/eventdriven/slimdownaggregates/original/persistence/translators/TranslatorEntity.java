package io.eventdriven.slimdownaggregates.original.persistence.translators;

import jakarta.persistence.*;

import java.util.UUID;

@Entity
@Table(name = "translators")
public class TranslatorEntity {
  @Id
  @GeneratedValue(strategy = GenerationType.AUTO)
  private UUID id;

  private String name;

  public TranslatorEntity() {
    // Default constructor for JPA
  }

  // Standard getters and setters

  public UUID getId() {
    return id;
  }

  public void setId(UUID id) {
    this.id = id;
  }

  public String getName() {
    return name;
  }

  public void setName(String lastName) {
    this.name = lastName;
  }
}
