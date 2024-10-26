package io.eventdriven.slimdownaggregates.original.persistence.publishers;

import jakarta.persistence.*;

import java.util.UUID;

@Entity
@Table(name = "publishers")
public class PublisherEntity {
  @Id
  @GeneratedValue(strategy = GenerationType.AUTO)
  private UUID id;

  private String name;

  public PublisherEntity() {
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
