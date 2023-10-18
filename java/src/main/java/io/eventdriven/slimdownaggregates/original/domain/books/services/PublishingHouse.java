package io.eventdriven.slimdownaggregates.original.domain.books.services;


import io.eventdriven.slimdownaggregates.original.domain.books.entities.Genre;

public interface PublishingHouse {
  boolean isGenreLimitReached(Genre genre);
}
