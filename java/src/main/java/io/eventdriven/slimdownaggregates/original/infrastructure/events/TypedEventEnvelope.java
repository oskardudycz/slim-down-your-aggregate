package io.eventdriven.slimdownaggregates.original.infrastructure.events;

public record TypedEventEnvelope<Event>(
  Event typedEvent,
  EventMetadata metadata
) implements EventEnvelope {

  @Override
  public Object event() {
    return this.typedEvent;
  }

  @Override
  public EventMetadata metadata() {
    return this.metadata;
  }
}
