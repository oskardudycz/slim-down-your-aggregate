package io.eventdriven.slimdownaggregates.book.services;

import io.eventdriven.slimdownaggregates.book.entities.Genre;

public interface IPublishingHouse {
  boolean canPublish(Genre genre);
}
