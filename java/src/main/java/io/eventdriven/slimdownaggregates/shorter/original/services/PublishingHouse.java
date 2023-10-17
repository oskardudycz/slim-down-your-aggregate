package io.eventdriven.slimdownaggregates.shorter.original.services;

import io.eventdriven.slimdownaggregates.shorter.original.entities.Genre;

public interface PublishingHouse {
  boolean isGenreLimitReached(Genre genre);
}
