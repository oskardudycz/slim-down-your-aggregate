package io.eventdriven.slimdownaggregates.slimmed.services;

import io.eventdriven.slimdownaggregates.slimmed.entities.Genre;

public interface PublishingHouse {
  boolean isGenreLimitReached(Genre genre);
}
