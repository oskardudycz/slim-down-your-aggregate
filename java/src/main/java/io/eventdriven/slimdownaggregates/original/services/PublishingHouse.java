package io.eventdriven.slimdownaggregates.original.services;

import io.eventdriven.slimdownaggregates.original.entities.Genre;

public interface PublishingHouse {
  boolean isGenreLimitReached(Genre genre);
}
