package io.eventdriven.slimdownaggregates.shorter.slimmed.services;

import io.eventdriven.slimdownaggregates.shorter.slimmed.entities.Genre;

public interface PublishingHouse {
  boolean isGenreLimitReached(Genre genre);
}
