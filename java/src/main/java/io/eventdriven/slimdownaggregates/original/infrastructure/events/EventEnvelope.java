package io.eventdriven.slimdownaggregates.original.infrastructure.events;

import io.eventdriven.slimdownaggregates.original.infrastructure.valueobjects.NonEmptyString;

public interface EventEnvelope {
  Object event();

  EventMetadata metadata();


  record EventMetadata(NonEmptyString recordId) {
  }
}

