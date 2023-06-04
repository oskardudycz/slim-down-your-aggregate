package io.eventdriven.slimdownaggregates.slimmed.services;

import io.eventdriven.slimdownaggregates.slimmed.entities.Genre;

public interface IPublishingHouse {
  boolean isGenreLimitReached(Genre genre);
}
