package io.eventdriven.slimdownaggregates.original.domain.books.publishers;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.Publisher;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.PublisherId;

public interface PublisherProvider {
  Publisher getById(PublisherId publisherId);
}
