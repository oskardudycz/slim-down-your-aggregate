package io.eventdriven.slimdownaggregates.original.infrastructure.aggregates;

import io.eventdriven.slimdownaggregates.original.infrastructure.events.DomainEvent;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import static io.eventdriven.slimdownaggregates.original.infrastructure.validation.Validation.assertNotNull;

public abstract class Aggregate<TKey> {

  protected TKey id;
  private final List<DomainEvent> domainEvents;

  protected Aggregate(TKey id) {
    assertNotNull(id);

    this.id = id;
    this.domainEvents = new ArrayList<>();
  }

  protected void addDomainEvent(DomainEvent domainEvent) {
    this.domainEvents.add(domainEvent);
  }

  public void clearEvents() {
    this.domainEvents.clear();
  }

  public List<DomainEvent> getDomainEvents() {
    return Collections.unmodifiableList(domainEvents);
  }

  public TKey getId() {
    return id;
  }
}
