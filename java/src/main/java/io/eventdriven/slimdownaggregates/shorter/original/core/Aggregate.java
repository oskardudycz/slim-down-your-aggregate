package io.eventdriven.slimdownaggregates.shorter.original.core;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.UUID;

public abstract class Aggregate {
  protected UUID id;
  private List<IDomainEvent> domainEvents;

  protected Aggregate(UUID id) {
    this.id = id;
    this.domainEvents = new ArrayList<>();
  }

  protected void addDomainEvent(IDomainEvent domainEvent) {
    this.domainEvents.add(domainEvent);
  }

  public void clearEvents() {
    this.domainEvents.clear();
  }

  public List<IDomainEvent> getDomainEvents() {
    return Collections.unmodifiableList(domainEvents);
  }

  public UUID getId() {
    return id;
  }
}

