package io.eventdriven.slimdownaggregates.original.persistence.publishers;

import io.eventdriven.slimdownaggregates.original.domain.books.entities.Publisher;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.PublisherId;
import io.eventdriven.slimdownaggregates.original.domain.books.entities.PublisherName;
import io.eventdriven.slimdownaggregates.original.domain.books.publishers.PublisherProvider;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional(readOnly = true)
public class PublisherService implements PublisherProvider {
  private final PublisherRepository publisherRepository;

  public PublisherService(PublisherRepository publisherRepository) {
    this.publisherRepository = publisherRepository;
  }

  public Publisher getById(PublisherId publisherId) {
    return publisherRepository.findById(publisherId.value())
      .map(p -> new Publisher(new PublisherId(p.getId()), new PublisherName(p.getName())))
      .orElseThrow(() -> new IllegalStateException("Publisher not found"));
  }
}
